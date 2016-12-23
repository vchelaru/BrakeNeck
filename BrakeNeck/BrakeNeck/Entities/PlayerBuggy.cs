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
	public partial class PlayerBuggy
	{
        /// <summary>
        /// The ratio that the car is moving relative to its max speed. Range is 0 to 1
        /// </summary>
        float currentSpeedRatio;

        // Start as a negative number so the bullet can be shot immediately
        double lastBulletShot = -100;

        public IPressableInput GasInput { get; set; }
        public I1DInput SteeringInput { get; set; }
        public I2DInput AimingInput { get; set; }
        public IPressableInput ShootingInput { get; set; }

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            //CreateKeyboardInput();

            CreateOnePlayerXboxControllerInput();

        }

        private void CreateOnePlayerXboxControllerInput()
        {
            var controller = InputManager.Xbox360GamePads[0];

            GasInput = controller.GetButton(Xbox360GamePad.Button.LeftTrigger);
            ShootingInput = controller.GetButton(Xbox360GamePad.Button.RightTrigger);

            AimingInput = controller.RightStick;
            SteeringInput = controller.LeftStick.Horizontal;
        }

        private void CreateKeyboardInput()
        {
            GasInput = InputManager.Keyboard.GetKey(Keys.W);
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
            if(GasInput.IsDown)
            {
                currentSpeedRatio += TimeManager.SecondDifference / TimeToSpeedUp;
            }
            else
            {
                currentSpeedRatio -= TimeManager.SecondDifference / TimeToSpeedUp;
            }

            currentSpeedRatio = Math.Min(1, currentSpeedRatio);
            currentSpeedRatio = Math.Max(0, currentSpeedRatio);

            this.Velocity = currentSpeedRatio * this.RotationMatrix.Up * MaxSpeed;
            
            var radianVelocity = MathHelper.ToRadians(RotationSpeed);
            this.RotationZVelocity = -this.SteeringInput.Value * radianVelocity * currentSpeedRatio;
        }

        private void PerformShootingInput()
        {
            float angle = TurretInstance.RotationZ + MathHelper.PiOver2;

            if(AimingInput.X != 0 || AimingInput.Y != 0)
            {
                angle = (float)Math.Atan2(AimingInput.Y, AimingInput.X);
                TurretInstance.RelativeRotationZ = angle - RotationZ - MathHelper.PiOver2;

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
            bullet.RotationZ = angle;
            bullet.Velocity = BulletSpeed * bullet.RotationMatrix.Right;
            
            bullet.Position = TurretInstance.Position + Turret.BulletOffset * TurretInstance.RotationMatrix.Up; 
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
