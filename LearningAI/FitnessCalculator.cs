namespace LearningAI
{
    public static class FitnessCalculator
    {
        private const int ReachedGoalPoints = 100000;
        private const int StepPoints = -1;
        private const int ObstacleHitPoints = -100;

        public static double CalculateFitness(bool reachedGoal, uint steps, int obstacleHits, int distanceToGoalSquared)
            => reachedGoal
                ? 5.0 + 10000.0 / ((steps + 10000 * obstacleHits) * (steps + 10000 * obstacleHits))
                : 1.0 / distanceToGoalSquared; //_reachedGoal ? ReachedGoalPoints : 0 + StepPoints * _steps + ObstacleHitPoints * _obstacleHits;
    }
}
