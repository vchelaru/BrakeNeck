
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using FlatRedBall.Localization;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Microsoft.Xna.Framework;

namespace BrakeNeck.Screens
{
	public partial class GameScreen
	{
        #region Initialize

        void CustomInitialize()
		{
            Camera.Main.BackgroundColor = Color.SandyBrown;

		}

        #endregion

        void CustomActivity(bool firstTimeCalled)
		{
            BulletDestructionActivity();

            ObstacleDestructionActivity();

            CollisionActivity();

            ScrollingActivity();

		}

        private void CollisionActivity()
        {
            PerformBulletVsBoxCollision();

            PerformPlayerVsBoxCollision();
        }

        private void PerformPlayerVsBoxCollision()
        {
            for (int obstacleIndex = ObstacleList.Count - 1; obstacleIndex > -1; obstacleIndex--)
            {
                var obstacle = ObstacleList[obstacleIndex];

                this.PlayerBuggyInstance.CollideAgainstMove(obstacle, 0, 1);
            }
        }

        private void PerformBulletVsBoxCollision()
        {
            for (int bulletIndex = PlayerBulletList.Count - 1; bulletIndex > -1; bulletIndex--)
            {
                var bullet = PlayerBulletList[bulletIndex];

                for (int obstacleIndex = ObstacleList.Count - 1; obstacleIndex > -1; obstacleIndex--)
                {
                    var obstacle = ObstacleList[obstacleIndex];

                    if (bullet.CollideAgainst(obstacle))
                    {

                        var particles = new Entities.Particles.MissileDestructionParticles();
                        particles.Position = bullet.Position;
                        particles.Emit();
                        MissileDestructionParticlesList.Add(particles);

                        bullet.Destroy();

                        obstacle.Health--;
                        if (obstacle.Health <= 0)
                        {
                            obstacle.Destroy();
                        }
                        break;
                    }

                }
            }
        }

        private void ObstacleDestructionActivity()
        {
            for(int i = ObstacleList.Count - 1; i > -1; i--)
            {
                var obstacle = ObstacleList[i];

                if(obstacle.Y < -1000 + Camera.Main.Y)
                {
                    obstacle.Destroy();
                }
            }
        }

        void ScrollingActivity()
        {
            float desiredTruckY = Camera.Main.Y - Camera.Main.OrthogonalHeight / 4;
            float heightAbove = this.PlayerBuggyInstance.Y - desiredTruckY;

            float velocity = Math.Max(0, heightAbove);

            var ratioOfMaxBuggyVelocity = velocity / PlayerBuggyInstance.MaxSpeed;

            this.ObstacleSpawnerInstance.SpawningRatio = ratioOfMaxBuggyVelocity;

            Camera.Main.Y += velocity * TimeManager.SecondDifference;
        }

        private void BulletDestructionActivity()
        {
            // Add/subtract 20 so the bullet is fully off screen
            float top = Camera.Main.AbsoluteTopYEdgeAt(0) + 20;
            float bottom = Camera.Main.AbsoluteBottomYEdgeAt(0) - 20;

            for (int i = PlayerBulletList.Count - 1; i > -1; i--)
            {
                // We'll use 1000 as the out-of-screen values:
                var bullet = PlayerBulletList[i];

                if(bullet.X > 1000 || bullet.X < -1000 ||
                   bullet.Y > top || bullet.Y < bottom)
                {
                    bullet.Destroy();
                }
            }
        }

        void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
