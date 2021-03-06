﻿using System;
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
        private int _generation = 1;
        private int _iteration = 1;
        private int _averageIteration;
        private DateTime _startOfIteration;

        public Population(int size, Point goal)
        {
            _dots = new List<Dot>(size);
            for (int i = 0; i < size; i++)
            {
                _dots.Add(new Dot(goal));
            }

            _startOfIteration = DateTime.Now;
        }

        public IEnumerable<DotPosition> GetDotPositions() => _dots.Select(d => d.GetDotPosition());

        private bool ShouldEndGeneration() => _dots.All(d => d.IsDead);

        public override string ToString() => $"Generation: {_generation}, iteration: {_iteration}, averageIteration: {_averageIteration}";

        public void Update()
        {
            if (ShouldEndGeneration())
            {
                    CalculateFitness();
                    NaturalSelection();
                    MutateBabies();
                    _iteration = 1;
                    _startOfIteration = DateTime.Now;
            }
            else
            {
                _iteration++;
                Parallel.ForEach(_dots, dot => dot.Update());

                _averageIteration = (int)(DateTime.Now - _startOfIteration).TotalMilliseconds / _iteration;
                const int maxIterationTimeInMilliseconds = 15;
                if (_averageIteration < maxIterationTimeInMilliseconds)
                {
                    Thread.Sleep(maxIterationTimeInMilliseconds);
                }
            }
        }

        private void CalculateFitness() => Parallel.ForEach(_dots, dot => dot.CalculateFitness());

        private void NaturalSelection()
        {
            var bestDot = _dots.WithMaximum(d => d.Fitness);
            double fitnessSum = _dots.Sum(d => d.Fitness);

            var parentIndexes = new ConcurrentBag<int>();
            Parallel.For(1, _dots.Count, i =>
            {
                parentIndexes.Add(SelectParent(_dots, fitnessSum));
            });

            var newDots = new List<Dot>(_dots.Count) {new Dot(bestDot)};
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
                runningSum += bestDots[i].Fitness;
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
    }
}