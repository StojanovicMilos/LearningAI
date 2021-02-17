using System.Diagnostics;
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

        public void Show() => _population.Show();

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