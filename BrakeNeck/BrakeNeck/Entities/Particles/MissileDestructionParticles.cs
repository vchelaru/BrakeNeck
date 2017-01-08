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

namespace BrakeNeck.Entities.Particles
{
	public partial class MissileDestructionParticles
	{
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
		}

        public void Emit()
        {
            for (int i = 0; i < NumberOfSprites; i++)
            {
                EmitSprite();
            }

            this.Call(Destroy).After(TimeLasting);

        }

        private void EmitSprite()
        {
            var sprite = SpriteManager.AddParticleSprite(null);
            sprite.AnimationChains = AnimationChainListFile;
            sprite.CurrentChainIndex = FlatRedBallServices.Random.Next(AnimationChainListFile.Count);
            sprite.TextureScale = 1;

            float rotationRadians = MathHelper.ToRadians(MaxRotationPerSecondDegrees);
            sprite.RotationZVelocity = rotationRadians - (float)FlatRedBallServices.Random.NextDouble() * 2 * rotationRadians;

            float offsetAngle = (float)FlatRedBallServices.Random.NextDouble() * MathHelper.TwoPi;
            var offset = new Vector3((float)Math.Cos(offsetAngle), (float)Math.Sin(offsetAngle), 0) * (float)(FlatRedBallServices.Random.NextDouble() * EmissionRadius);
            sprite.Position = this.Position + offset;

            float movementAngle = (float)FlatRedBallServices.Random.NextDouble() * MathHelper.TwoPi;
            sprite.Velocity = new Vector3((float)Math.Cos(movementAngle), (float)Math.Sin(movementAngle), 0) * (float)(FlatRedBallServices.Random.NextDouble() * MaxSubtleMovementSpeed);


            Sprites.Add(sprite);
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
	}
}
