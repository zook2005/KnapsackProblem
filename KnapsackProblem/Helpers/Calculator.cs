namespace KnapsackProblem.Helpers
{
    public static class Calculator
    {
        /// <summary>
        ///This function gets the total number of unique combinations based upon N and K.
        /// N is the total number of items.
        /// K is the size of the group.
        /// Total number of unique combinations = N! / ( K! (N - K)! ).
        /// This function is less efficient, but is more likely to not overflow when N and K are large.
        /// Taken from:  http://blog.plover.com/math/choose.html
        ///</summary>
        /// <param name="N"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        public static long Choose(long N, long K)
        {
            long r = 1;
            long d;
            if (K > N) return 0;
            for (d = 1; d <= K; d++)
            {
                r *= N--;
                r /= d;
            }
            return r;
        }
    }
}
