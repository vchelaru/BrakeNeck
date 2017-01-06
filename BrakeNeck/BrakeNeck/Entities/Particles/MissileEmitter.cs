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
using FlatRedBall.Screens;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities.Particles
{
	public partial class MissileEmitter
	{
        double lastEmission;

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
            var screen = ScreenManager.CurrentScreen;

            var frequency = 1 / ParticlesPerSecond;

            if (screen.PauseAdjustedSecondsSince(lastEmission) > frequency)
            {
                EmitParticle();
            }

		}

        private void EmitParticle()
        {
            var randomIndex = FlatRedBallServices.Random.Next(AnimationChainListFile.Count);
            var animationChain = AnimationChainListFile[randomIndex];
            var sprite = SpriteManager.AddParticleSprite(null);
            sprite.SetAnimationChain(animationChain);
            if (LayerProvidedByContainer != null)
            {
                SpriteManager.AddToLayer(sprite, this.LayerProvidedByContainer);
            }

            sprite.Position = this.Position;
            sprite.X += PositionSpread - 2 * PositionSpread * (float)FlatRedBallServices.Random.NextDouble(); 
            sprite.Y += PositionSpread - 2 * PositionSpread * (float)FlatRedBallServices.Random.NextDouble(); 

            sprite.XVelocity = -MaxVelocity + 2*MaxVelocity * (float)FlatRedBallServices.Random.NextDouble();
            sprite.YVelocity = -MaxVelocity + 2*MaxVelocity * (float)FlatRedBallServices.Random.NextDouble();
            sprite.Drag = 1;

            sprite.TextureScale = 1;

            var screen = ScreenManager.CurrentScreen;

            // assocaite them with the screen:
            screen.Call(() => SpriteManager.RemoveSprite(sprite)).After(ParticleDuration);
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
