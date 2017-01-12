using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using KnapsackProblem.Knapsack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;
using QuickGraph.Algorithms;
using KnapsackProblem.GeneticPack;
// ReSharper disable JoinDeclarationAndInitializer

namespace Tests
{
    [TestClass]
    public class GraphsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var edges = new SEdge<int>[] {
            //    new SEdge<int>(0, 1),
            //    new SEdge<int>(1, 2),
            //};
            //var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
            //var dictionary = new Dictionary<int, int>();
            //graph.WeaklyConnectedComponents(dictionary);
            //////graph.CondensateWeaklyConnected<>();
            ////var list = dictionary.ToList();
            ////list.RemoveAll(kvp => kvp.Key == kvp.Value);
            ////dictionary = list.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            //
            //IDictionary<int, int> dictionary2 = new Dictionary<int, int>();
            //graph.StronglyConnectedComponents(out dictionary2);
            //
            //var components = AlgorithmExtensions.IncrementalConnectedComponents(graph);
            //
            //var current = components();


            // a simple adjacency graph representation
            int[][] graph = new int[5][];
            graph[0] = new int[] { 1 };
            graph[1] = new int[] { 2, 3 };
            graph[2] = new int[] { 3, 4 };
            graph[3] = new int[] { 4 };
            graph[4] = new int[] { };

            // interoping with quickgraph
            var g = GraphExtensions.ToDelegateVertexAndEdgeListGraph(
                Enumerable.Range(0, graph.Length),
                v => Array.ConvertAll(graph[v], w => new SEquatableEdge<int>(v, w))
                );

 
            //var condensated = g.CondenstateWeaklyConnected();


        }

        [TestMethod]
        public void TestMethod2()
        {
            List<int> firstParent;
            List<int> secondParent;
            int lowerBoundry;
            int upperBoundry;
            List<int> firstChild;
            List<int> secondChild;
            Tuple<List<int>, List<int>> tuple;
            var god = new God(100);

            lowerBoundry = 2;
            upperBoundry = 6;

            //firstParent = new List<Item>() {1, 2, 3, 4, 5, 7, 6, 8, 9};
            //secondParent = new List<int>() {2, 3, 7, 6, 4, 5, 8, 9, 1};
            //firstChild = god.CrossOver(new Population());

        }

        [TestMethod]
        public void TestMethod3()
        {
            List<int> firstParent;
            List<int> secondParent;
            int lowerBoundry;
            int upperBoundry;
            List<int> firstChild;
            List<int> secondChild;
            Tuple<List<int>, List<int>> tuple;


            lowerBoundry = 2;
            upperBoundry = 6;

            firstParent = new List<int>() { 1, 2, 3, 4, 5, 7, 6, 8, 9 };
            secondParent = new List<int>() { 2, 3, 7, 6, 4, 5, 8, 9, 1 };
            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);
            
            Assert.IsTrue(firstChild.SequenceEqual(new List<int>() { 1, 2, 7, 6, 4, 5, 3, 8, 9 }));


            firstParent = new List<int>() { 1, 2, 6, 4, 5, 7, 3, 8, 9 };
            secondParent = new List<int>() { 2, 3, 7, 6, 4, 5, 8, 9, 1 };
            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);
            
            Assert.IsTrue(firstChild.SequenceEqual(new List<int>() { 1, 2, 7, 6, 4, 5, 3, 8, 9 }));


            firstParent = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            secondParent = new List<int>() { 2, 1, 4, 5, 6, 7, 3, 9, 8 };

            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);            
            secondChild = foo1(secondParent, firstParent, lowerBoundry, upperBoundry);
            Assert.IsTrue(firstChild.SequenceEqual(new List<int>() { 1, 2, 4, 5, 6, 7, 3, 8, 9 }));
            Assert.IsTrue(secondChild.SequenceEqual(new List<int>() { 2, 1, 3, 4, 5, 6, 7, 9, 8 }));



            lowerBoundry = upperBoundry;
            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);
            Assert.IsTrue(firstChild.SequenceEqual(firstParent));


            lowerBoundry = upperBoundry = 0;
            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);
            Assert.IsTrue(firstChild.SequenceEqual(firstParent));


            lowerBoundry = upperBoundry = firstParent.Count;
            firstChild = foo1(firstParent, secondParent, lowerBoundry, upperBoundry);
            Assert.IsTrue(firstChild.SequenceEqual(firstParent));




        }

        private List<int> foo1(List<int> firstParent, List<int> secondParent, int lowerBoundry, int upperBoundry)
        {
            List<int> firstChild = new List<int>(firstParent);

            var firstMap = new Dictionary<int, int>();

            for (int k = lowerBoundry; k < upperBoundry; k++)
            {
                //create an Edge between two arrays, at index k
                firstMap.Add(secondParent[k], firstParent[k]);
                //switch places of elements on the same index (children's list, parents do not change)
                firstChild[k] = secondParent[k];
            }


            foo(lowerBoundry, upperBoundry, firstChild, firstMap, null);
            return firstChild;
        }

        private void foo(int lowerBoundry, int upperBoundry, List<int> firstChild, Dictionary<int, int> currentMap, Dictionary<int, int> lastMap)
        {
            var reminder = new Dictionary<int, int>();
            foreach (var kvp in currentMap.ToList())
            {
                //search kvp.key outside the [lowerBoundry,upperBoundry] segment. once found replace it with kvp.value and move on to next kvp. if not found, keep it in reminder and search it again in the next recursion step.
                var found = false;

                for (int k = 0; k < lowerBoundry; k++)
                {
                  
                    var x = kvp.Key;
                    if (firstChild[k] == x)
                    {
                        found = true;
                        firstChild[k] = kvp.Value; //switch
                        currentMap.Remove(x);//update current map
                        break;
                    }
                }
                if(found) continue;
                for (int k = upperBoundry; k < firstChild.Count; k++)
                {
                    var x = kvp.Key;
                    if (firstChild[k] == x)
                    {
                        found = true;
                        firstChild[k] = kvp.Value; //switch
                        currentMap.Remove(x);//update current map
                        break;
                    }
                }
                if (found) continue;
                reminder.Add(kvp.Key, kvp.Value); //Edge was not used in this call. keep it to next call

            }
            //if (reminder.ToList().SequenceEqual(currentMap.ToList())) return;
            if (lastMap != null && currentMap.ToList().SequenceEqual(lastMap.ToList())) return;

            foo(lowerBoundry,upperBoundry,firstChild, reminder, currentMap);
        }
    }
}
