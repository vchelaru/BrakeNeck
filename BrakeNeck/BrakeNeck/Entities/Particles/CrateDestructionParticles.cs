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

namespace BrakeNeck.Entities.Particles
{
	public partial class CrateDestructionParticles
	{
        public float AreaWidth { get; set;  }
        public float AreaHeight { get; set; }

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
            ParticleActivity();

            DestructionActivity();

        }

        private void ParticleActivity()
        {
            const float yOffsetPerZ = -12f;
            const float xOffsetPerZ = -12f;

            for(int i = 0; i < ShadowSprites.Count; i++)
            {
                var shadowSprite = ShadowSprites[i];
                var cratePiece = CratePieceSprites[i];

                cratePiece.X = shadowSprite.X + xOffsetPerZ * (shadowSprite.Z - cratePiece.Z);
                cratePiece.Y = shadowSprite.Y + yOffsetPerZ * (shadowSprite.Z - cratePiece.Z);
                cratePiece.RotationZ = shadowSprite.RotationZ;

                bool shouldBounce = cratePiece.Z <= shadowSprite.Z && cratePiece.ZVelocity < 0;
                cratePiece.Z = Math.Max(shadowSprite.Z + .001f, cratePiece.Z);
                if(shouldBounce)
                {
                    const float bounceVelocityMultiplier = .35f;

                    cratePiece.ZVelocity *= -bounceVelocityMultiplier;

                    shadowSprite.Velocity *= bounceVelocityMultiplier;
                }
            }
        }

        private void DestructionActivity()
        {
            const float distanceBeforeDestroying = 1000;

            // Leave a lot of room here 
            var cutoff = Camera.Main.AbsoluteBottomYEdgeAt(0) - distanceBeforeDestroying;
            if (this.Y < cutoff)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        internal void Emit()
        {
            for (int i = 0; i < NumberOfSprites; i++)
            {
                EmitSprite();
            }
        }

        private void EmitSprite()
        {
            var sprite = SpriteManager.AddParticleSprite(null);
            GameScreen.ResetParticle(sprite);

            sprite.Drag = 0;
            sprite.AnimationChains = AnimationChainListFile;
            sprite.CurrentChainIndex = FlatRedBallServices.Random.Next(AnimationChainListFile.Count);
            sprite.TextureScale = 1;

            sprite.Position = this.Position;

            Vector2 offset = new Vector2(
                FlatRedBallServices.Random.Between(-AreaWidth / 2, AreaWidth / 2),
                FlatRedBallServices.Random.Between(-AreaHeight / 2, AreaHeight / 2)
                );


            sprite.ZVelocity = FlatRedBallServices.Random.Between(10, 30);
            sprite.ZAcceleration = -60;

            CratePieceSprites.Add(sprite);

            // Shadow Sprite will be the dominant sprite - the other sprite will be positioned using the shadow:
            var shadowSprite = SpriteManager.AddParticleSprite(null);
            GameScreen.ResetParticle(shadowSprite);

            shadowSprite.Drag = 0;
            shadowSprite.AnimationChains = AnimationChainListFile;
            shadowSprite.CurrentChainIndex = sprite.CurrentChainIndex;
            shadowSprite.TextureScale = 1;
            shadowSprite.ColorOperation = FlatRedBall.Graphics.ColorOperation.ColorTextureAlpha;

            shadowSprite.Alpha = .3f;

            float rotationRadians = MathHelper.ToRadians(MaxRotationPerSecondDegrees);
            sprite.RotationZVelocity =  rotationRadians - (float)FlatRedBallServices.Random.NextDouble() * 2 * rotationRadians;


            shadowSprite.Red = 0;
            shadowSprite.Green = 0;
            shadowSprite.Blue = 0;


            shadowSprite.Position = this.Position;
            shadowSprite.X += offset.X;
            shadowSprite.Y += offset.Y;

            const float velocityOffsetMultiplier = 5;

            shadowSprite.XVelocity = offset.X * velocityOffsetMultiplier;
            shadowSprite.YVelocity = offset.Y * velocityOffsetMultiplier;

            ShadowSprites.Add(shadowSprite);            
        }
    }
}
