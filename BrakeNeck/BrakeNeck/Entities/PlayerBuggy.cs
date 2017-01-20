#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using Microsoft.Xna.Framework;
using FlatRedBall.Screens;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
    public enum ShotType
    {
        Straight,
        Spread,
        Thick
    }


	public partial class PlayerBuggy
	{
        /// <summary>
        /// The ratio that the car is moving relative to its max speed. Range is 0 to 1
        /// </summary>
        float currentSpeedRatio;

        // Start as a negative number so the bullet can be shot immediately
        double lastBulletShot = -100;

        public IPressableInput GasInput { get; set; }
        public IPressableInput ReverseInput { get; set; }
        public I1DInput SteeringInput { get; set; }
        public I2DInput AimingInput { get; set; }
        public IPressableInput ShootingInput { get; set; }

        public ShotType ShotType { get; set; } = ShotType.Straight;

        public int Score { get; set; }
        public int Multiplier { get; set; } = 1;

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            if(Screens.MainMenu.SelectedInputDevice == Screens.InputDevice.Controller)
            {
                CreateXboxControllerInput(0);
            }
            else
            {
                CreateKeyboardInput();
            }


        }

        public void CreateXboxControllerInput(int index)
        {
            var controller = InputManager.Xbox360GamePads[index];

            GasInput = controller.GetButton(Xbox360GamePad.Button.LeftTrigger);
            ShootingInput = controller.GetButton(Xbox360GamePad.Button.RightTrigger);
            ReverseInput = controller.GetButton(Xbox360GamePad.Button.DPadDown);

            AimingInput = controller.RightStick;
            SteeringInput = controller.LeftStick.Horizontal;

            // If we wanted to support multiplayer or selecting colors, we would
            // have more sophisticated code, but for now, just tie it to controller index
            if(index == 1)
            {
                this.SpriteInstance.CurrentChainName = "TruckAnimation2";
            }
        }

        private void CreateKeyboardInput()
        {
            GasInput = InputManager.Keyboard.GetKey(Keys.W);
            ReverseInput = InputManager.Keyboard.GetKey(Keys.S);
            SteeringInput = InputManager.Keyboard.Get1DInput(Keys.A, Keys.D);

            AimingInput = InputManager.Keyboard.Get2DInput(Keys.J, Keys.L, Keys.I, Keys.K);
            ShootingInput = InputManager.Keyboard.GetKey(Keys.LeftControl);
        }

        private void CustomActivity()
		{
            PerformMovementInput();

            PerformShootingInput();

		}

        private void PerformMovementInput()
        {
            if(currentSpeedRatio > 0)
            {
                if(GasInput.IsDown)
                {
                    currentSpeedRatio += TimeManager.SecondDifference / TimeToSpeedUp;
                }
                else
                {
                    currentSpeedRatio -= TimeManager.SecondDifference / TimeToSlowDown;
                    if(currentSpeedRatio < 0)
                    {
                        currentSpeedRatio = 0;
                    }
                }
            }
            else if(currentSpeedRatio < 0)
            {
                if (ReverseInput.IsDown)
                {
                    currentSpeedRatio -= TimeManager.SecondDifference / TimeToSpeedUp;
                }
                else
                {
                    currentSpeedRatio += TimeManager.SecondDifference / TimeToSlowDown;
                    if(currentSpeedRatio > 0)
                    {
                        currentSpeedRatio = 0;
                    }
                }
            }
            else
            {
                if (GasInput.IsDown)
                {
                    currentSpeedRatio += TimeManager.SecondDifference / TimeToSpeedUp;
                }
                if (ReverseInput.IsDown)
                {
                    currentSpeedRatio -= TimeManager.SecondDifference / TimeToSpeedUp;
                }
            }

            currentSpeedRatio = Math.Min(1, currentSpeedRatio);
            currentSpeedRatio = Math.Max(-1, currentSpeedRatio);

            this.BackLeftTire.AnimationSpeed = currentSpeedRatio;
            this.BackRightTire.AnimationSpeed = currentSpeedRatio;
            this.FrontLeftTire.AnimationSpeed = currentSpeedRatio;
            this.FrontRightTire.AnimationSpeed = currentSpeedRatio;

            this.Velocity = currentSpeedRatio * this.RotationMatrix.Right * MaxSpeed;
            
            var radianVelocity = MathHelper.ToRadians(RotationSpeed);

            // We want to allow this guy to turn even when standing still:
            var speedRatioForTurning = Math.Max(.2f, Math.Abs(currentSpeedRatio));
            if(currentSpeedRatio < 0)
            {
                speedRatioForTurning *= -1;
            }

            this.RotationZVelocity = -this.SteeringInput.Value * radianVelocity * speedRatioForTurning;
            this.TurnRatio = this.SteeringInput.Value;
        }

        private void PerformShootingInput()
        {
            float angle = TurretInstance.RotationZ;

            if(AimingInput.X != 0 || AimingInput.Y != 0)
            {
                angle = (float)Math.Atan2(AimingInput.Y, AimingInput.X);
                TurretInstance.RelativeRotationZ = angle - RotationZ;

            }

            var screen = ScreenManager.CurrentScreen;

            if (this.ShootingInput.IsDown && 
                screen.PauseAdjustedSecondsSince(lastBulletShot) > 1/TurretInstance.BulletsPerSecond)
            {
                lastBulletShot = screen.PauseAdjustedCurrentTime;

                this.ShootBullet(angle);
            }
        }

        private void ShootBullet(float angle)
        {
            var bullet = Factories.PlayerBulletFactory.CreateNew();
            
            var position = TurretInstance.Position + Turret.BulletOffset * TurretInstance.RotationMatrix.Right; 

            bullet.Z += .1f;

            switch (this.ShotType)
            {
                case ShotType.Straight:
                    // do nothing:
                    break;
                case ShotType.Spread:
                    angle += FlatRedBallServices.Random.Between(-.2f, .2f);
                    break;
                case ShotType.Thick:
                    position += FlatRedBallServices.Random.Between(-46, 46) * this.TurretInstance.RotationMatrix.Up;
                    break;
            }

            bullet.Position = position;
            bullet.RotationZ = angle;
            bullet.Velocity = BulletSpeed * bullet.RotationMatrix.Right;
        }

        internal void UpdateForwardVelocity()
        {
            var forwardVector = this.RotationMatrix.Right;
            var projected = Vector3.Dot(forwardVector, Velocity);

            this.currentSpeedRatio = projected / MaxSpeed;
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
