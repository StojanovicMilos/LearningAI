using System.Drawing;

namespace LearningAI
{
    public class Dot
    {
        private int _positionX = 400;
        private int _positionY = 780;
        private Velocity _velocity = new Velocity(0, 0);
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
        private bool _reachedGoal;
        public Fitness Fitness { get; private set; }

        public DotPosition GetDotPosition() => new DotPosition {X = _positionX, Y = _positionY, IsBest = _isBest};

        private void Move()
        {
            if (_velocity.HasValue())
            {
                var (velocityX, velocityY) = _velocity.GetNext();
                _positionX += velocityX;
                _positionY += velocityY;
                return;
            }

            if (_brain.HasDirections)
            {
                var acceleration = _brain.GetNextDirection();
                _velocity = _velocity.Add(acceleration);
                return;
            }

            IsDead = true;
        }

        public void CalculateFitness() => Fitness = new Fitness(_reachedGoal, _brain.Step, 0, DistanceToGoalSquared());

        private uint DistanceToGoalSquared() => (uint) ((_positionX - _goal.X) * (_positionX - _goal.X) + (_positionY - _goal.Y) * (_positionY - _goal.Y));

        public void Update()
        {
            if (IsDead || _reachedGoal)
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
                _reachedGoal = true;
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