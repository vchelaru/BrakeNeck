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
using FlatRedBall.Math;

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


#if DEBUG
        double[] LastTwoSpawnTimes = new double[2];
        // to smooth it out:
        RollingAverage SpawnRollingAverage = new RollingAverage(4);
#endif

        public float MovementRatio { get; set; } = 1;
        public string DebugText
        {
            get
            {

                return $"Spawn every {this.YDistanceBetweenSpawns / SpawnRateMultiple} units\n" +
                    $"Spawn every {(SpawnRollingAverage.Average).ToString("0.000")} seconds";
            }
        }

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

            SpawnRateMultiple += SpawnRateMultipleVelocity * TimeManager.SecondDifference;

        }

        
        private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }


        public Obstacle PerformSpawn()
        {
#if DEBUG
            var screen = FlatRedBall.Screens.ScreenManager.CurrentScreen;
            LastTwoSpawnTimes[0] = LastTwoSpawnTimes[1];
            LastTwoSpawnTimes[1] = screen.PauseAdjustedCurrentTime;

            SpawnRollingAverage.AddValue((float)(LastTwoSpawnTimes[1] - LastTwoSpawnTimes[0]));

#endif
            var newObstacle = ObstacleFactory.CreateNew();

            if(FlatRedBallServices.Random.Between(0, 1) < RatioOfGold)
            {
                newObstacle.CurrentBonusCategoryState = Obstacle.BonusCategory.Bonus;
            }

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
