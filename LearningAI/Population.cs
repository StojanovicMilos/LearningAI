using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningAI
{
    public class Population
    {
        private List<Dot> _dots;
        private int _generation;
        private int _iteration = 1;
        private int _averageIteration;
        private DateTime _startOfIteration;

        public Population(int numberOfDots, Point goal, int generation)
        {
            _dots = DotsFactory.GetDots(numberOfDots, goal);

            _startOfIteration = DateTime.Now;
            _generation = generation;
        }

        public IEnumerable<DotPosition> GetDotPositions() => _dots.Select(d => d.GetDotPosition());

        public override string ToString() => $"Generation: {_generation}, iteration: {_iteration-1}, averageIteration: {_averageIteration}";

        public void Update()
        {
            UpdateDots();
            ControlSpeed();

            if (ShouldEndGeneration())
            {
                _iteration = 1;
                CalculateFitness();
                NaturalSelection();
                MutateBabies();
                _startOfIteration = DateTime.Now;
            }
            else
            {
                _iteration++;
            }
        }
        private bool ShouldEndGeneration() => _dots.All(d => d.IsDead);

        private void CalculateFitness() => Parallel.ForEach(_dots, dot => dot.CalculateFitness());

        private void NaturalSelection()
        {
            var bestDot = _dots.WithMaximumScore();
            double fitnessSum = _dots.Sum(d => d.FitnessScore);

            var parentIndexes = new ConcurrentBag<int>();
            Parallel.For(1, _dots.Count, i =>
            {
                parentIndexes.Add(SelectParent(_dots, fitnessSum));
            });

            var newDots = new List<Dot>(_dots.Count) { new Dot(bestDot) };
            newDots.AddRange(parentIndexes.Select(parentIndex => _dots[parentIndex].CreateBaby()));

            _dots = newDots;

            _generation++;
        }

        private readonly Random _random = new Random();

        private int SelectParent(IReadOnlyList<Dot> bestDots, double fitnessSum)
        {
            double randomFitness = _random.NextDouble() * fitnessSum;

            double runningSum = 0;
            for (int i = 0; i < bestDots.Count; i++)
            {
                runningSum += bestDots[i].FitnessScore;
                if (runningSum > randomFitness)
                    return i;
            }

            throw new InvalidOperationException("The code should not reach here");
        }

        private void MutateBabies()
        {
            foreach (var dot in _dots.Skip(1))
            {
                dot.MutateBrain();
            }
        }

        private void UpdateDots()
        {
            foreach (var dot in _dots)
            {
                dot.Update();
            }
        }

        private void ControlSpeed()
        {
            _averageIteration = (int)(DateTime.Now - _startOfIteration).TotalMilliseconds / _iteration;
            const int maxIterationTimeInMilliseconds = 25;
            if (_averageIteration < maxIterationTimeInMilliseconds)
            {
                Thread.Sleep(maxIterationTimeInMilliseconds);
            }
        }

        public void Save()
        {
            PopulationFactory.Save(_generation);
            DotsFactory.SaveDirection(_dots.OrderByDescending(dot => dot.FitnessScore).Select(dot => dot.GetDirection()));
        }
    }
}