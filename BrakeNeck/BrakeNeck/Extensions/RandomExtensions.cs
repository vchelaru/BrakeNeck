using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrakeNeck.Extensions
{
    public static class RandomExtensions
    {
        public static float FloatInRange(this Random rand, float min, float max)
        {
            float range = max - min;
            float chosen = (float)(rand.NextDouble() * range);
            return min + chosen;
        }
    }
}
