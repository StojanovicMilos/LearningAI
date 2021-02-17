using System;
using System.Drawing;

namespace LearningAI
{
    public static class DirectionsProvider
    {
        private static readonly Random Random = new Random();

        public static Point GetRandomDirection()
        {
            
            int x;
            int y;
            do
            {
                x = Random.Next(-1, 2);
                y = Random.Next(-1, 2);
            } while (x == 0 && y == 0);

            return new Point(x, y);
        }
    }
}