using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.Helpers
{
    public static class IoHelpers
    {
        /// <summary>
        /// parsing file to extract all "Items"
        /// </summary>
        /// <param name="path"></param>
        /// <returns>All Items in file</returns>
        public static IEnumerable<Item> ParseInputFile(string path)
        {
            //todo: handle path\file\format exceptions. parsing exception
            using (var reader = File.OpenText(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var regex = new Regex(@"^\s*(?<id>\d+)\s*(?<size>\d+)\s*(?<value>\d+)");

                    var match = regex.Match(line);
                    if (!match.Success) continue;
                    var groups = match.Groups;
                    var id = UInt32.Parse(groups["id"].Value);
                    var size = UInt32.Parse(groups["size"].Value);
                    var value = UInt32.Parse(groups["value"].Value);

                    var item = new Item(id, size, value);
                    yield return item;
                }
            }
        }

        /// <summary>
        /// creates a file with the format of an input file. returns the path for the created 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="directory"> where to save the file</param>
        /// <returns>a path to the created file</returns>
        public static string CreateInputFile(IEnumerable<Item> items, string  directory ,string filename)
        {
            string path;

            if (directory == null /*|| !new DirectoryInfo(path).Exists*/)
            {
                directory = Directory.GetCurrentDirectory();
            }
            if (filename == null) filename = Path.GetRandomFileName();

            path = Path.Combine(directory, filename);

            File.WriteAllLines(path, items.Where(i=>i!=null).Select(i=>i.ToOutportFormat()));

            return path;
        }

        /// <summary>
        /// creates a file with the format of an input file. returns the path for the file created 
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a path to the created file</returns>
        public static string CreateInputFile(IEnumerable<Item> items)
        {
            string path;
            var directory = Directory.GetCurrentDirectory();
            path = Path.Combine(directory, Path.GetRandomFileName());  
            File.WriteAllLines(path, items.Where(i=>i!=null).Select(i=>i.ToOutportFormat()));
            return path;
        }

        public static IEnumerable<Item> CreateItemsRandomly(uint numOfItems, uint itemsMaxSize, uint maxvalue)
        {
            var rand = new Random();
            for (uint i = 0; i < numOfItems; i++)
                yield return new Item(i, (uint)rand.Next(1, (int)(maxvalue + 1)), (uint)rand.Next(1, (int)(itemsMaxSize + 1)));
        }

        public static string CreateOutputFile(IEnumerable<Item> items, string directory, string filename)
        {
            string path;

            if (directory == null /*|| !new DirectoryInfo(path).Exists*/)
            {
                directory = Directory.GetCurrentDirectory();
            }
            if (filename == null) filename = Path.GetRandomFileName();

            path = Path.Combine(directory, filename);

            File.WriteAllLines(path, items.Where(i => i != null).Select(i => i.ToString()));

            return path;
        }

        public static string CreateOutputFile(string inputFilePath)
        {
            string outputFilePath;
            //create an output file

            var directory = Path.GetDirectoryName(inputFilePath);
            var filename = Path.GetFileNameWithoutExtension(inputFilePath) + "- output";

            if (directory == null /*|| !new DirectoryInfo(path).Exists*/)
            {
                directory = Directory.GetCurrentDirectory();
            }
            if (filename == null) filename = Path.GetRandomFileName();

            outputFilePath = Path.Combine(directory, filename);
            return outputFilePath;
        }
    }
}