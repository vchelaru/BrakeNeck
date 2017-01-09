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
using BrakeNeck.Screens;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
	public partial class GroundDecorationSpawner
	{
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


		}

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public void RemoveOffScreenDecorations()
        {
            var absoluteBottom = Camera.Main.AbsoluteBottomYEdgeAt(0);
            for(int i = SpawnedDecorations.Count - 1; i > -1; i--)
            {
                var decoration = SpawnedDecorations[i];
                if (decoration.Top < absoluteBottom)
                {
                    SpriteManager.RemoveSprite(decoration);
                }
            }
        }

        public void PerformSpawn()
        {
            var sprite = SpriteManager.AddParticleSprite(null);
            GameScreen.ResetParticle(sprite);
            sprite.AnimationChains = AnimationChainListFile;
            sprite.CurrentChainIndex = FlatRedBallServices.Random.Next(AnimationChainListFile.Count);
            sprite.TextureScale = 1;

            int numberOfRotations = FlatRedBallServices.Random.Next(4);
            this.PositionNewObstacle(sprite);
            sprite.RotationZ = MathHelper.ToRadians(numberOfRotations * 90);

            SpawnedDecorations.Add(sprite);

            this.lastSpawnY = Y;
        }
    }
}
