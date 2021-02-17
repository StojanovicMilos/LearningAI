using System.Collections.Generic;

namespace LearningAI
{
    public interface IPopulation
    {
        IEnumerable<DotPosition> GetDotPositions();
        void Update();
    }
}