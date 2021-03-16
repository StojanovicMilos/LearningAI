using System;
using System.IO;

namespace LearningAI
{
    public static class PopulationFactory
    {
        private const string DirectoryName = "Save";
        private const string FilePath = DirectoryName + @"\Population.txt";

        public static int GetGeneration()
        {
            return File.Exists(FilePath) ? Convert.ToInt32(File.ReadAllLines(FilePath)[0]) : 1;
        }

        public static void Save(in int generation)
        {
            Directory.CreateDirectory(DirectoryName);
            File.WriteAllText(FilePath, generation.ToString());
        }
    }
}