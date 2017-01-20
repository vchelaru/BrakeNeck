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

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
	public partial class Obstacle
	{


        public float Width
        {
            get
            {
                return AxisAlignedRectangleInstance.Width;
            }
        }

        public float Height
        {
            get
            {
                return AxisAlignedRectangleInstance.Height;
            }
        }

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            // randomly rotate it
            var rotationAmount = FlatRedBallServices.Random.Next(4) * 90;

            this.SpriteInstance.RelativeRotationZ =
                Microsoft.Xna.Framework.MathHelper.ToRadians(rotationAmount);


        }

		private void CustomActivity()
		{


		}

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        /// <summary>
        /// Takes a hit and returns whether the box was destroyed.
        /// </summary>
        /// <returns>Whether the box was destroyed</returns>
        public bool TakeHit()
        {
            bool wasDestroyed = false;
            Health--;
            if (Health <= 0)
            {
                Destroy();
                wasDestroyed = true;
            }
            else if(Health == 1)
            {
                this.SpriteInstance.CurrentChainName = "Damage2";
            }
            else if (Health == 2)
            {
                this.SpriteInstance.CurrentChainName = "Damage1";
            }
            return wasDestroyed;
        }
	}
}
