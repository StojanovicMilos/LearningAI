using System;
using System.Drawing;

namespace LearningAI
{
    public static class DirectionsProvider
    {
        private static readonly Random Random = new Random();
        private const int MaximumAcceleration = Velocity.MaximumVelocity / 4;

        public static Point GetRandomDirection()
        {
            int x;
            int y;
            do
            {
                x = Random.Next(-MaximumAcceleration, MaximumAcceleration + 1);
                y = Random.Next(-MaximumAcceleration, MaximumAcceleration + 1);
            } while (x == 0 && y == 0);

            return new Point(x, y);
        }
    }
}