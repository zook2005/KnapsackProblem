using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace Tests
{
    [TestClass]
    public class IoHelpersTests
    {
        [TestMethod]
        public void ParseInputFileTest()
        {
            var items = IoHelpers.ParseInputFile(@"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\simpleFile4Testing.txt");
            Assert.AreEqual(items.Count(),3);
        }

        [TestMethod]
        public void CreateItemsRandomlyTest()
        {
            var items = IoHelpers.CreateItemsRandomly(100, 10, 10);
            var outputpath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\simpleFile4Testing-output222.txt";
            var path = IoHelpers.CreateInputFile(items, Path.GetDirectoryName(outputpath), Path.GetFileName(outputpath));
        }

        [TestMethod]
        public void CreateInputFileTest()
        {
            var inputFile =
                @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\simpleFile4Testing.txt";
            var items = IoHelpers.ParseInputFile(inputFile);
            string outputpath;
            //outputpath = null;
            outputpath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\simpleFile4Testing-output.txt";
            var path = IoHelpers.CreateInputFile(items, Path.GetDirectoryName(outputpath), Path.GetFileName(outputpath));

            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(File.ReadAllLines(inputFile).SequenceEqual(File.ReadAllLines(path).Where(line=>!string.IsNullOrWhiteSpace(line))));
        }

        [TestMethod]
        public void CreateInput()
        {
            var outputpath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input C a.txt";
            List<Item> items = new List<Item>();
            for (uint n = 1; n <= 200; n++)
            {
                var f_n = Math.Max(n + 10, Math.Max(3 * n + 8, 5 * n + 2));

                items.Add(new Item(n,n,f_n));
            }

            IoHelpers.CreateInputFile(items, Path.GetDirectoryName(outputpath), Path.GetFileName(outputpath));
        }
        [TestMethod]
        public void CreateInput2()
        {
            var outputpath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input F a.txt";
            List<Item> items = new List<Item>();
            uint n = 1;
            for (;n <= 30; n++)
            {
                items.Add(new Item(n, n * 4, n*4));
            }
            var moreItems = new uint[] {11, 13, 15, 17};
            for (var x = 0; x < moreItems.Length; x++, n++)
            {
                var item = moreItems[x];
                items.Add(new Item(n, item, item));
            }

            IoHelpers.CreateInputFile(items, Path.GetDirectoryName(outputpath), Path.GetFileName(outputpath));
        }
        [TestMethod]
        public void CreateInput3()
        {
            var outputpath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input G a.txt";
            List<Item> items = new List<Item>();
            uint id = 0;
			
            for (uint n = 5; n <= 54; n++)
            {
				for(int j = 0; j<10;j++)
				{
					items.Add(new Item(id, n , n*3+7));
					id++;
				}
            }

            IoHelpers.CreateInputFile(items, Path.GetDirectoryName(outputpath), Path.GetFileName(outputpath));
        }
    }
}
