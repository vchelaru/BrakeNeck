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
using FlatRedBall.Math;
using Microsoft.Xna.Framework;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
	public partial class Tire
	{
        PositionedObjectList<Sprite> treads = new PositionedObjectList<Sprite>();

        Vector3 lastPositionSpawned;

        

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
            TreadSpawningActivity();

            TreadRemovalActivity();
        }

        private void TreadRemovalActivity()
        {
            var cameraBottom = Camera.Main.AbsoluteBottomYEdgeAt(0) - 20;
            for(int i = treads.Count - 1; i > -1; i--)
            {
                var tread = treads[i];
                if(tread.Y < cameraBottom)
                {
                    SpriteManager.RemoveSprite(tread);
                }
            }
        }

        private void TreadSpawningActivity()
        {
            bool shouldSpawn = (Position - lastPositionSpawned).LengthSquared() > 15 * 15;
            if (shouldSpawn)
            {
                CreateTread();
                lastPositionSpawned = Position;
            }
        }

        private void CustomDestroy()
		{

		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        private void CreateTread()
        {
            const int treadOffset = 7;

            var leftSprite = SpriteManager.AddManualParticleSprite(null);
            leftSprite.AnimationChains = AnimationChainListFile;
            leftSprite.CurrentChainName = nameof(TreadLeft);
            leftSprite.TextureScale = 1;

            leftSprite.RotationZ = this.RotationZ - MathHelper.PiOver2;
            leftSprite.Position = this.Position + leftSprite.RotationMatrix.Left * treadOffset;

            SpriteManager.ManualUpdate(leftSprite);

            this.treads.Add(leftSprite);

            var rightSprite = SpriteManager.AddManualParticleSprite(null);
            rightSprite.AnimationChains = AnimationChainListFile;
            rightSprite.CurrentChainName = nameof(TreadRight);
            rightSprite.TextureScale = 1;

            rightSprite.RotationZ = this.RotationZ - MathHelper.PiOver2;
            rightSprite.Position = this.Position + rightSprite.RotationMatrix.Right * treadOffset;

            SpriteManager.ManualUpdate(rightSprite);

            this.treads.Add(rightSprite);




        }
    }
}
