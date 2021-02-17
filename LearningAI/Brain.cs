using System;
using System.Drawing;
using System.Linq;

namespace LearningAI
{
    public class Brain
    {
        private Point[] _directions;
        private readonly Random _random = new Random();

        public Brain(uint size)
        {
            _directions = new Point[size];
            for (int i = 0; i < _directions.Length; i++)
            {
                _directions[i] = DirectionsProvider.GetRandomDirection();
            }
        }

        private Brain() { }

        public bool HasDirections => Step < _directions.Length;
        public uint Step { get; private set; }

        public Point GetNextDirection() => _directions[Step++];

        public Brain Copy() => new Brain {_directions = _directions.Select(d => new Point(d.X, d.Y)).ToArray()};

        public void Mutate()
        {
            const double mutationRate = 0.01;

            for (int i = 0; i < _directions.Length; i++)
            {
                if (mutationRate > _random.NextDouble())
                {
                    _directions[i] = DirectionsProvider.GetRandomDirection();
                }
            }
        }
    }
}