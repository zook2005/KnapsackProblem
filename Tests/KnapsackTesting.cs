using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.GeneticPack;
using KnapsackProblem.Knapsack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnapsackProblem.Helpers;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Tests
{
    [TestClass]
    public class KnapsackTesting
    {
      
        [TestMethod]
        public void TestMethod1()
        {
            var _allItems = IoHelpers.CreateItemsRandomly(100, 10, 10).ToList();
            var _knapsack = Knapsacks.KnapsackForGenetic(_allItems, 100);
            Assert.IsTrue(_knapsack.Even);
            //var pop = new Population(_allItems,2,100);

            var edges = new SEdge<int>[] {
                new SEdge<int>(1, 2),
                new SEdge<int>(0, 1)
            };
            var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
            var dictionary = new Dictionary<int, int>();
            graph.WeaklyConnectedComponents(dictionary);

        }
    }
}
