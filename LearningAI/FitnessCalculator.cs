namespace LearningAI
{
    public static class FitnessCalculator
    {
        private const int ReachedGoalPoints = 100000;
        private const int StepPoints = -1;
        private const int ObstacleHitPoints = -100;

        public static double CalculateFitness(bool reachedGoal, uint steps, uint obstacleHits, int distanceToGoalSquared) 
            => reachedGoal ? 0.0625 + 10000.0 / (steps * steps) : 1.0 / distanceToGoalSquared; //_reachedGoal ? ReachedGoalPoints : 0 + StepPoints * _steps + ObstacleHitPoints * _obstacleHits;
    }
}
