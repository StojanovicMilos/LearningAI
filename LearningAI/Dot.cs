using System.Drawing;

namespace LearningAI
{
    public class Dot
    {
        private int _positionX = 400;
        private int _positionY = 780;
        private int _velocityX;
        private int _velocityY;
        private Point _acceleration = new Point(0, 0);
        private readonly bool _isBest;
        private readonly Point _goal;
        private bool _reachedGoal;
        
        public Dot(Point goal)
        {
            _goal = goal;
            Brain = new Brain(1000);
        }

        public Dot(Dot dot)
        {
            _goal = dot._goal;
            Brain = dot.Brain.Copy();
            _isBest = true;
        }

        public bool IsDead { get; set; }
        public Brain Brain { get; private set; }

        public double Fitness { get; private set; }

        public DotPosition GetDotPosition()
        {
            return new DotPosition
            {
                X = _positionX, Y = _positionY, IsBest = _isBest
            };
        }

        private void Move()
        {
            if (Brain.HasDirections)
            {
                _acceleration = Brain.GetNextDirection();
            }
            else
            {
                IsDead = true;
                return;
            }

            _velocityX += _acceleration.X;
            if (_velocityX > 5)
            {
                _velocityX = 5;
            }
            else if (_velocityX < -5)
            {
                _velocityX = -5;
            }

            _velocityY += _acceleration.Y;
            if (_velocityY > 5)
            {
                _velocityY = 5;
            }
            else if (_velocityY < -5)
            {
                _velocityY = -5;
            }

            _positionX += _velocityX;
            _positionY += _velocityY;
        }

        public void CalculateFitness()
        {
            if (_reachedGoal)
            {
                Fitness = 1.0 / 16.0 + 10000.0 / (Brain.Step * Brain.Step);
            }
            else
            {
                Fitness = 1.0 / DistanceToGoalSquared();
            }
        }

        private double DistanceToGoalSquared() => (_positionX - _goal.X) * (_positionX - _goal.X) +
                                                  (_positionY - _goal.Y) * (_positionY - _goal.Y);

        public void Update()
        {
            if (IsDead || _reachedGoal) return;

            Move();
            if (_positionX < 2 || _positionY < 2 || _positionX > 798 || _positionY > 798)
            {
                IsDead = true;
            }
            else if (DistanceToGoalSquared() < 25) // less than 5 pixels from goal
            {
                _reachedGoal = true;
                IsDead = true;
            }
            else if (_positionX < 600 && _positionY < 310 && _positionX > 0 && _positionY > 300)
            {
                IsDead = true;
            }
            else if (_positionX < 800 && _positionY < 510 && _positionX > 200 && _positionY > 500)
            {
                IsDead = true;
            }
        }

        public Dot CreateBaby() => new Dot(_goal) { Brain = Brain.Copy() };

        public void MutateBrain() => Brain.Mutate();
    }
}