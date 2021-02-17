using System.Collections.Generic;
using System.Threading;

namespace LearningAI
{
    public class SynchronizingPopulation : IPopulation
    {
        private readonly IPopulation _population;
        private readonly Mutex _mutex;

        public SynchronizingPopulation(IPopulation population, Mutex mutex)
        {
            _population = population;
            _mutex = mutex;
        }


        public IEnumerable<DotPosition> GetDotPositions() => _population.GetDotPositions();

        public void Update()
        {
            while (true)
            {
                _mutex.WaitOne();
                _population.Update();
                _mutex.ReleaseMutex();
            }
        }

        public override string ToString() => _population.ToString();
    }
}