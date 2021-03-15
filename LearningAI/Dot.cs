using System.Drawing;
using System.Linq;

namespace LearningAI
{
    public class Dot
    {
        private int _positionX = 400;
        private int _positionY = 780;
        private Velocity _velocity = new Velocity(0, 0);
        private readonly bool _isBest;
        private readonly Point _goal;
        private int _obstaclesHit;

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
        public double FitnessScore { get; private set; }

        public DotPosition GetDotPosition() => new DotPosition { X = _positionX, Y = _positionY, IsBest = _isBest };

        public void CalculateFitness() => FitnessScore = FitnessCalculator.CalculateFitness(_reachedGoal, _brain.Step, _obstaclesHit, DistanceToGoalSquared());

        private int DistanceToGoalSquared() => (_positionX - _goal.X) * (_positionX - _goal.X) + (_positionY - _goal.Y) * (_positionY - _goal.Y);

        public void Update()
        {
            if (!_brain.HasDirections)
            {
                IsDead = true;
                return;
            }

            var acceleration = _brain.GetNextDirection();
            _velocity = _velocity.Add(acceleration);

            while (_velocity.HasValue())
            {
                if (IsDead || _reachedGoal)
                {
                    return;
                }

                if (!_brain.HasDirections && !_velocity.HasValue())
                {
                    IsDead = true;
                    return;
                }

                var (velocityX, velocityY) = _velocity.GetNext();
                _positionX += velocityX;
                _positionY += velocityY;

                if (DistanceToGoalSquared() <= DotPosition.Diameter)
                {
                    _reachedGoal = true;
                    IsDead = true;
                    return;
                }

                var dotPosition = GetDotPosition();
                foreach (var obstacle in Obstacles.Get().Where(obstacle => obstacle.Hits(dotPosition)))
                {
                    _obstaclesHit++;
                    obstacle.UpdateVelocity(dotPosition, _velocity);
                }
            }

            
        }

        public Dot CreateBaby() => new Dot(_goal) { _brain = _brain.Copy() };

        public void MutateBrain() => _brain.Mutate();
    }
}