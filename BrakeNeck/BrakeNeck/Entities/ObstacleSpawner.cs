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
using BrakeNeck.Factories;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

#endif
#endregion

namespace BrakeNeck.Entities
{
	public partial class ObstacleSpawner
	{
        // Start at 0 to give some time before the first spawn
        double lastSpawnY = 0;

        /// <summary>
        /// The ratio at which spawning should happen. At 1, 
        /// the obstacle spawner will spawn at full speed. At
        /// 0, it will completely stop spawning. This is used to
        /// make spawning happen according to camera movement.
        /// </summary>
        public float SpawningRatio { get; set; }

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
#if DEBUG
            if(this.YDistanceBetweenSpawns == 0)
            {
                throw new InvalidOperationException("ObstaclesPerSecond cannot be 0");
            }
#endif

            if(this.Y > lastSpawnY + this.YDistanceBetweenSpawns)
            {
                PerformSpawn();

                lastSpawnY = this.Y;
            }
		}

        private void PerformSpawn()
        {
            var newObstacle = ObstacleFactory.CreateNew();
            newObstacle.Y = this.Y;
            var width = Camera.Main.OrthogonalWidth;
            newObstacle.X = -width / 2.0f + (float)FlatRedBallServices.Random.NextDouble() * width;
        }

        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
	}
}
