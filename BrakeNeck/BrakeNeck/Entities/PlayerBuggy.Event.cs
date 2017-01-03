using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using BrakeNeck.Entities;
using BrakeNeck.Screens;
using Microsoft.Xna.Framework;

namespace BrakeNeck.Entities
{
	public partial class PlayerBuggy
	{
        void OnAfterTurnRatioSet (object sender, EventArgs e)
        {
            if(TurnRatio != 0)
            {
                int m = 3;
            }
            float maxRotationRadians = MathHelper.ToRadians(MaxTireTurnDegrees);
            FrontLeftTire.RelativeRotationZ = -maxRotationRadians * TurnRatio;
            FrontRightTire.RelativeRotationZ = -maxRotationRadians * TurnRatio;
        }

    }
}
