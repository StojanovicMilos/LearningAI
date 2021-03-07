namespace LearningAI
{
    public class Fitness
    {
        private const int ReachedGoalPoints = 100000;
        private const int StepPoints = -1;
        private const int ObstacleHitPoints = -100;

        private readonly bool _reachedGoal;
        private readonly uint _steps;
        private readonly uint _obstacleHits;
        private readonly uint _distanceToGoalSquared;

        public Fitness(bool reachedGoal, uint steps, uint obstacleHits, uint distanceToGoalSquared)
        {
            _reachedGoal = reachedGoal;
            _steps = steps;
            _obstacleHits = obstacleHits;
            _distanceToGoalSquared = distanceToGoalSquared;
        }

        public double Score() => _reachedGoal ? 0.0625 + 10000.0 / (_steps * _steps) : 1.0 / _distanceToGoalSquared; //_reachedGoal ? ReachedGoalPoints : 0 + StepPoints * _steps + ObstacleHitPoints * _obstacleHits;
    }
}
