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

using BrakeNeck.Extensions;
#endregion

namespace BrakeNeck.Entities
{
	public partial class SandStormEffect
	{
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            RandomizeSpinSpeeds();

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

        private void RandomizeSpinSpeeds()
        {
            SandSprite1.RelativeRotationZVelocity = -FlatRedBallServices.Random.FloatInRange(RotationVelocityMin, RotationVelocityMax);
            SandSprite2.RelativeRotationZVelocity = -FlatRedBallServices.Random.FloatInRange(RotationVelocityMin, RotationVelocityMax);
            SandSprite3.RelativeRotationZVelocity = -FlatRedBallServices.Random.FloatInRange(RotationVelocityMin, RotationVelocityMax);
            SandSprite4.RelativeRotationZVelocity = -FlatRedBallServices.Random.FloatInRange(RotationVelocityMin, RotationVelocityMax);
            SandSprite5.RelativeRotationZVelocity = -FlatRedBallServices.Random.FloatInRange(RotationVelocityMin, RotationVelocityMax);
        }
	}
}
