using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LearningAI
{
    public class Population : IPopulation
    {
        private Dot[] _dots;
        private int _generation = 1;

        public Population(uint size, Point goal)
        {
            _dots = new Dot[size];
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i] = new Dot(goal);
            }
        }

        public IEnumerable<DotPosition> GetDotPositions() => _dots.Select(d => d.GetDotPosition());

        private bool ShouldEndGeneration() => _dots.All(d => d.IsDead);

        public override string ToString() => $"Generation: {_generation}, iteration: {_dots.Max(d => d.Brain.Step)}";

        public void Update()
        {
            if (ShouldEndGeneration())
            {
                CalculateFitness();
                NaturalSelection();
                MutateBabies();
            }
            else
            {
                foreach (var dot in _dots)
                {
                    if (!dot.Brain.HasDirections)
                    {
                        dot.IsDead = true;
                    }
                    else
                    {
                        dot.Update();
                    }
                }
            }
        }

        private void CalculateFitness()
        {
            foreach (var dot in _dots)
            {
                dot.CalculateFitness();
            }
        }

        private void NaturalSelection()
        {
            Dot[] newDots = new Dot[_dots.Length];
            var bestDot = _dots.WithMaximum(d => d.Fitness);
            newDots[0] = new Dot(bestDot);

            var bestDots = _dots.Where(d => d.Fitness >= 0.9 * bestDot.Fitness).ToArray();
            double fitnessSum = bestDots.Sum(d => d.Fitness);


            for (int i = 1; i < newDots.Length; i++)
            {
                Dot parent = SelectParent(bestDots, fitnessSum);
                newDots[i] = parent.CreateBaby();
            }

            _dots = newDots;
            _generation++;
        }

        private readonly Random _random = new Random();

        private Dot SelectParent(Dot[] bestDots, double fitnessSum)
        {
            double randomFitness = _random.NextDouble() * fitnessSum;

            double runningSum = 0;
            foreach (var dot in bestDots)
            {
                runningSum += dot.Fitness;
                if (runningSum > randomFitness)
                    return dot;
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