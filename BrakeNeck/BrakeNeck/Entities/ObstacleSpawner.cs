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
        /// <summary>
        /// The spawner will create a path by keeping an
        /// x value. If any crate is within a certain distance
        /// of the path, it will attempt to reposition itself, making
        /// it less likely that crates will be along the path.
        /// </summary>
        float pathX;

        float pathXVelocity;

        public float MovementRatio { get; set; } = 1;

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            this.PathXDisplay.Visible = DebuggingVariables.ShowPath;

            StartMovingPath();
		}

        private void StartMovingPath()
        {
            float edgeX = Camera.Main.AbsoluteRightXEdgeAt(0);

            float endX = -edgeX + 2 * edgeX * (float)FlatRedBallServices.Random.NextDouble();

            const int timeToTake = 5;

            pathXVelocity = (endX - pathX) / timeToTake;

            this.Call(StartMovingPath).After(timeToTake);
        }

        private void CustomActivity()
		{
            pathX += pathXVelocity * TimeManager.SecondDifference * MovementRatio;

            PathXDisplay.RelativeX = pathX;
            PathXDisplay.RelativeY = Camera.Main.AbsoluteTopYEdgeAt(0) - this.Y;
		}

        
        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }


        public Obstacle PerformSpawn()
        {
            var newObstacle = ObstacleFactory.CreateNew();

            int repositionsSoFar = 0;
            while(repositionsSoFar <= NumberOfTimesToRepositionCrates)
            {
                PositionNewObstacle(newObstacle);

                var distanceFromCenterOfPath = Math.Abs(newObstacle.X - pathX) - newObstacle.Width;

                bool isOnPath = distanceFromCenterOfPath < PathWidth / 2.0f;

                if(!isOnPath)
                {
                    break;
                }

                repositionsSoFar++;
            }



            lastSpawnY = this.Y;

            return newObstacle;
        }
    }
}
