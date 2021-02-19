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

        private Brain _brain;

        public Dot(Point goal)
        {
            _goal = goal;
            _brain = new Brain(1000);
        }

        public Dot(Dot dot)
        {
            _goal = dot._goal;
            _brain = dot._brain.Copy();
            _isBest = true;
        }

        public bool IsDead { get; private set; }
        public bool ReachedGoal { get; private set; }
        public double Fitness { get; private set; }

        public DotPosition GetDotPosition() => new DotPosition {X = _positionX, Y = _positionY, IsBest = _isBest};

        private void Move()
        {
            if (_brain.HasDirections)
            {
                _acceleration = _brain.GetNextDirection();
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

        public void CalculateFitness() => Fitness = ReachedGoal ? 0.0625 + 10000.0 / (_brain.Step * _brain.Step) : 1.0 / DistanceToGoalSquared();

        private double DistanceToGoalSquared() => (_positionX - _goal.X) * (_positionX - _goal.X) + (_positionY - _goal.Y) * (_positionY - _goal.Y);

        public void Update()
        {
            if (IsDead || ReachedGoal)
            {
                return;
            }

            if (!_brain.HasDirections)
            {
                IsDead = true;
                return;
            }

            Move();
            if (DistanceToGoalSquared() < 4)
            {
                ReachedGoal = true;
                IsDead = true;
            }
            else if (Obstacles.AnyHitBy(GetDotPosition()))
            {
                IsDead = true;
            }
        }

        public Dot CreateBaby() => new Dot(_goal) { _brain = _brain.Copy() };

        public void MutateBrain() => _brain.Mutate();
    }
}