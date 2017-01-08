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
using FlatRedBall.Glue.StateInterpolation;
using FlatRedBall.Screens;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
	public partial class CameraController
	{

        /// <summary>
        /// A value of 0 to 1, controlling whether the shaking is not happening at all (0) or full intensity (1)
        /// </summary>
        public float ProximityRatio { get; set; } = 1;

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{

        }

        private void CustomActivity()
		{
            // Increasing this makes the camera orbit faster
            var rotationCoefficient = 6;
            // Increasing this makes the camera shake more
            // If this is too small (like < 10) then the rotation
            // becomes too obvious
            var shakeCoefficient = 24;

            var screen = ScreenManager.CurrentScreen;
            var time = (float)screen.PauseAdjustedCurrentTime;
            var angle = time * rotationCoefficient;

            var sineValue = (float)screen.PauseAdjustedCurrentTime * shakeCoefficient;
            var offset = (float)Math.Sin(sineValue);


            CameraInstance.RelativeX = (float)Math.Cos(angle) * offset * MaxShakeIntensity * ProximityRatio;
            CameraInstance.RelativeY = (float)Math.Sin(angle) * offset * MaxShakeIntensity * ProximityRatio;
        }

		private void CustomDestroy()
		{
            this.CameraInstance.Detach();

		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
