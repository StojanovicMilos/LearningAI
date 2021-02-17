using System;
using System.Drawing;
using System.Linq;

namespace LearningAI
{
    public class Population : IPopulation
    {
        private Dot[] _dots;
        private int _generation = 1;

        public Population(uint size, Graphics graphics, Point goal)
        {
            _dots = new Dot[size];
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i] = new Dot(graphics, goal);
            }
        }

        public void Show()
        {
            foreach (var dot in _dots)
            {
                dot.Show();
            }
        }

        private bool ShouldEndGeneration() => _dots.All(d => d.IsDead);
        
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
            int bestDotIndex = GetBestDotIndex();
            double fitnessSum = CalculateFitnessSum();

            newDots[0] = new Dot(_dots[bestDotIndex]);
            
            for (int i = 1; i < newDots.Length; i++)
            {
                Dot parent = SelectParent(fitnessSum);
                newDots[i] = parent.CreateBaby();
            }

            _dots = newDots;
            _generation++;
        }

        public override string ToString() => $"Generation: {_generation}, iteration: {_dots.Max(d => d.Brain.Step)}";

        private Dot SelectParent(double fitnessSum)
        {
            Random random = new Random();
            double randomFitness = random.NextDouble() * fitnessSum;

            double runningSum = 0;
            foreach (var dot in _dots.OrderBy(d=> random.NextDouble()))
            {
                runningSum += dot.Fitness;
                if (runningSum > randomFitness)
                    return dot;
            }

            return _dots.Last();
        }

        private double CalculateFitnessSum()
        {
            return _dots.Sum(d => d.Fitness);
        }

        private int GetBestDotIndex()
        {
            double max = 0;
            int maxIndex = 0;
            for (int i = 0; i < _dots.Length; i++)
            {
                if (_dots[i].Fitness > max)
                {
                    max = _dots[i].Fitness;
                    maxIndex = i;
                }
            }


            return maxIndex;
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