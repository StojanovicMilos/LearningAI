using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LearningAI
{
    public static class DotsFactory
    {
        private const string DirectoryName = "Save";
        private const string FilePath = DirectoryName + @"\Directions.txt";

        public static void SaveDirection(IEnumerable<Point[]> directions)
        {
            Directory.CreateDirectory(DirectoryName);
            var stringValue = directions.Select(direction => string.Join(';', direction.Select(d => $"{d.X},{d.Y}")));
            File.WriteAllLines(FilePath, stringValue);
        }

        public static List<Dot> GetDots(int numberOfDots, Point goal) =>
            File.Exists(FilePath)
                ? File
                    .ReadAllLines(FilePath)
                    .Select(line =>
                        new Dot(goal,
                            line.Split(';').Select(direction =>
                                    direction.Split(','))
                                .Select(parts => new Point(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1])))
                                .ToArray()
                        )
                    )
                    .ToList()
                : Enumerable.Range(0, numberOfDots).Select(_ => new Dot(goal, Enumerable.Range(0, Brain.Size)
                        .Select(__ => DirectionsProvider.GetRandomDirection()).ToArray()))
                    .ToList();
    }
}