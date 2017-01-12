using System;
using System.Collections.Generic;
using KnapsackProblem.Knapsack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class LoaclSearchTests
    {
        [TestMethod]
        public void InputA_Test()
        {
            /*
                C=126
                n=180
                20 פריטים מכל סוג:
                p1=1 w1=2
                p2=2 w2=3
                p3=3 w3=4
                p4=4 w4=7
                p5=8 w5=20
                p6=16 w6=44
                p7=32 w7=90
                p8=50 w8=100
                p9=64 w9=200
            */
            uint size = 126;
            List<Tuple<uint, uint>> itemsSources = new List<Tuple<uint, uint>>()
            {
                new Tuple<uint, uint>(1,2),
                new Tuple<uint, uint>(2,3),
                new Tuple<uint, uint>(3,4),
                new Tuple<uint, uint>(4,7),
                new Tuple<uint, uint>(8,20),
                new Tuple<uint, uint>(16,44),
                new Tuple<uint, uint>(32,90),
                new Tuple<uint, uint>(50,100),
                new Tuple<uint, uint>(64,200),
            };

            List<Item> items = new List<Item>();
            uint id = 0;
            foreach (var source in itemsSources)
            {
                for (int i = 0; i < 20; i++)
                {


                    items.Add(new Item(id, source.Item1, source.Item2));
                    id++;
                }
            }
            //IoHelpers.CreateInputFile(items, @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles");
            //LocalSearch localSearch = new LocalSearch();
            //var res = localSearch.Algorithm(items, size);
        }

        [TestMethod]
        public void InputB_Test()
        {
            /*
                
                C=630
                n=90
                15 פריטים מכל סוג:
                p1=3 w1=2
                p2=7 w2=3
                p3=16 w3=10
                p4=30 w4=21
                p5=63 w5=50
                p6=127 w6=100
            */
            uint size = 630;
            List<Tuple<uint, uint>> itemsSources = new List<Tuple<uint, uint>>()
            {
                new Tuple<uint, uint>(3,2),
                new Tuple<uint, uint>(7,3),
                new Tuple<uint, uint>(16,10),
                new Tuple<uint, uint>(30,21),
                new Tuple<uint, uint>(63,50),
                new Tuple<uint, uint>(127,100),
            };

            List<Item> items = new List<Item>();
            uint id = 0;
            foreach (var source in itemsSources)
            {
                for (int i = 0; i < 15; i++)
                {


                    items.Add(new Item(id, source.Item1, source.Item2));
                    id++;
                }
            }
            //IoHelpers.CreateInputFile(items, @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles");
            //LocalSearch localSearch = new LocalSearch();
            //var res = localSearch.Algorithm(items, size);
        }
    }
}
