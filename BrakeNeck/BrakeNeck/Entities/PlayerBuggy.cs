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
            CreateKeyboardInput();

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
            float movementRatio = 0;
            if(GasInput.IsDown)
            {
                movementRatio = 1;
            }

            this.Velocity = movementRatio * this.RotationMatrix.Up * MaxSpeed;
            
            var radianVelocity = MathHelper.ToRadians(RotationSpeed);
            this.RotationZVelocity = -this.SteeringInput.Value * radianVelocity * movementRatio;
        }

        private void PerformShootingInput()
        {
            if(this.ShootingInput.IsDown)
            {
                var angle = (float)Math.Atan2(AimingInput.Y, AimingInput.X);
                this.ShootBullet(angle);
            }
        }

        private void ShootBullet(float angle)
        {
            var bullet = Factories.PlayerBulletFactory.CreateNew();
            bullet.RotationZ = angle;
            bullet.Velocity = BulletSpeed * bullet.RotationMatrix.Right;
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
