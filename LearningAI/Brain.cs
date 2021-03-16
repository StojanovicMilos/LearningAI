using System;
using System.Drawing;
using System.Linq;

namespace LearningAI
{
    public class Brain
    {
        public Point[] Directions { get; }
        private readonly Random _random = new Random();

        public const int Size = 1000;

        public Brain(Point[] directions)
        {
            if (directions == null) throw new ArgumentNullException(nameof(directions));
            if(directions.Length != Size)
                throw new ArgumentOutOfRangeException(nameof(directions));

            Directions = directions;
        }

        public bool HasDirections => Step < Directions.Length;
        public uint Step { get; private set; }

        public Point GetNextDirection() => Directions[Step++];

        public Point[] CopyDirections() => Directions.Select(d => new Point(d.X, d.Y)).ToArray();

        public void Mutate()
        {
            const double mutationRate = 0.01;

            for (int i = 0; i < Directions.Length; i++)
            {
                if (mutationRate > _random.NextDouble())
                {
                    Directions[i] = DirectionsProvider.GetRandomDirection();
                }
            }
        }
    }
}