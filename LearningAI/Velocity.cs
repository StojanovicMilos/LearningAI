using System.Drawing;

namespace LearningAI
{
    public class Velocity
    {
        public const int MaximumVelocity = 5;

        private readonly int[] _velocitiesX = new int[MaximumVelocity];
        private readonly int[] _velocitiesY = new int[MaximumVelocity];
        private int _totalX;
        private int _totalY;
        private int _index;

        public Velocity(int x, int y) => SetInternals(x, y);

        private void SetInternals(int x, int y)
        {
            _index = 0;
            if (x < -MaximumVelocity)
            {
                x = -MaximumVelocity;
            }

            if (x > MaximumVelocity)
            {
                x = MaximumVelocity;
            }

            if (y < -MaximumVelocity)
            {
                y = -MaximumVelocity;
            }

            if (y > MaximumVelocity)
            {
                y = MaximumVelocity;
            }

            _totalX = x;
            _totalY = y;

            for (int i = 0; i < MaximumVelocity; i++)
            {
                int valueX;
                int valueY;

                if (x == 0)
                {
                    valueX = 0;
                }
                else if (x > 0)
                {
                    valueX = 1;
                    x--;
                }
                else
                {
                    valueX = -1;
                    x++;
                }

                if (y == 0)
                {
                    valueY = 0;
                }
                else if (y > 0)
                {
                    valueY = 1;
                    y--;
                }
                else
                {
                    valueY = -1;
                    y++;
                }

                _velocitiesX[i] = valueX;
                _velocitiesY[i] = valueY;
            }
        }

        public bool HasValue() => _index < _velocitiesX.Length;

        public (int, int) GetNext() => (_velocitiesX[_index], _velocitiesY[_index++]);

        public Velocity Add(Point acceleration) => new Velocity(_totalX + acceleration.X, _totalY + acceleration.Y);

        public void InvertX() => SetInternals(-_totalX, _totalY);

        public void InvertY() => SetInternals(_totalX, -_totalY);

        public void InvertBoth() => SetInternals(-_totalX, -_totalY);
    }
}
