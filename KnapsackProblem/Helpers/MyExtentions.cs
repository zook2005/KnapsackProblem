using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KnapsackProblem.Helpers
{
    /// <summary>
    /// a thread safe randomizer
    /// code copied from StackOverflow: http://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random _local;

        public static Random ThisThreadsRandom => _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }

    public static class MyExtensions
    {
        /// <summary>
        /// snuffles an IList by swapping elements randomly
        /// code copied from StackOverflow: http://stackoverflow.com/questions/273313/randomize-a-listt
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IList<T> CopyAndSwap<T>(this IList<T> original,int i, int j)
        {
            if (i < 0 || j < 0 || i > original.Count || j > original.Count) throw new ArgumentOutOfRangeException();
            if (original.Count == 0)  return original.ToList();
            var copy = original.ToList();
            copy[i] = original.ElementAt(j);
            copy[j] = original.ElementAt(i);
            return copy;
        }

        /// <summary>
        /// choose an element by the probability of his value 
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="individuals"></param>
        /// <returns></returns>
        public static int ChooseByProbability(double sum, ICollection<uint> individuals)
        {
            var p = ThreadSafeRandom.ThisThreadsRandom.NextDouble();
            double x = 0.0;

            var index = 0;
            while (index < individuals.Count)
            {
                x += individuals.ElementAt(index) / sum;
                if (x > p) break;
                index++;
            }
            return index;
        }

        public static T ChooseRandomElement<T>(this IEnumerable<T> @this) 
        {
            if (!@this.Any()) return default(T);
            var i = ThreadSafeRandom.ThisThreadsRandom.Next(@this.Count());
            return @this.ElementAtOrDefault(i);
        }

        public static double CalculateStdDev(this IEnumerable<double> values)
        {
            return Math.Sqrt(values.CalculateVariance());
        }

        /// <summary>
        /// calculates the variation of values.
        /// code copied from StackOverflow: http://stackoverflow.com/questions/3141692/standard-deviation-of-generic-list 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double CalculateVariance(this IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Any())
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }
}
