using System.Diagnostics;
using System.Threading;

namespace LearningAI
{
    public class SynchronizingPopulation : IPopulation
    {
        private readonly IPopulation _population;
        private readonly Mutex _mutex = new Mutex();

        public SynchronizingPopulation(IPopulation population)
        {
            _population = population;
        }

        public void Show()
        {
            _mutex.WaitOne();
            _population.Show();
            _mutex.ReleaseMutex();
        }

        public void Update()
        {
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Reset();
                _mutex.WaitOne();
                stopwatch.Start();
                _population.Update();
                stopwatch.Stop();
                _mutex.ReleaseMutex();
                var elapsed = (int)stopwatch.ElapsedMilliseconds;
                if (elapsed < 15)
                {
                    Thread.Sleep(15-elapsed);
                }
            }
        }

        public override string ToString()
        {
            try
            {
                _mutex.WaitOne();
                return _population.ToString();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}