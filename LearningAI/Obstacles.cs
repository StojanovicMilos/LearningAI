using System.Collections.Generic;
using System.Linq;

namespace LearningAI
{
    public static class Obstacles
    {
        private static readonly IEnumerable<Obstacle> obstacles = new Obstacle[]
        {
            new Obstacle(0, 0, 800, 10),
            new Obstacle(0, 0, 10, 800),
            new Obstacle(790, 0, 10, 800),
            new Obstacle(0, 790, 800, 10),

            new Obstacle(0, 200, 600, 10), 
            new Obstacle(200, 400, 600, 10),
            new Obstacle(0, 600, 600, 10)
        };

        public static IEnumerable<Obstacle> Get() => obstacles;

        public static bool AnyHitBy(DotPosition dotPosition) => obstacles.Any(o => o.Hits(dotPosition));
    }
}
