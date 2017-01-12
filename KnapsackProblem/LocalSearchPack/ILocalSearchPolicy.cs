using System.Collections.Generic;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.LocalSearchPack
{
    public interface ILocalSearchPolicy
    {
        bool SearchLocally(NeighborhoodManager neighborhoodManager);
    }

    public class ChooseBest : ILocalSearchPolicy
    {
        public bool SearchLocally(NeighborhoodManager neighborhoodManager)
        {
            var originalSolution = neighborhoodManager.OriginalSolution;

            //loop through all neighbors looking for the best solution
            bool found = false;
            SolutionDiff bestcandidate = new SolutionDiff();
            while (neighborhoodManager.MoveNext())
            {
                //step 1. get a legal(!) candidate
                SolutionDiff candidate = neighborhoodManager.Current;

                //step 2. add more items to candidate, to make it better, while keeping it legal (Even number of items)
                //this.AddMoreItemsToCandidate(candidate, neighborhoodManager);

                if (originalSolution.WouldBeImprovedBy(candidate)) //if candidate is legal
                {
                    if (candidate.CompareTo(bestcandidate) > 0) //if candidate is better than best candidate
                    {
                        bestcandidate = candidate;
                        found = true;
                    }
                }
            }

            if (found)
            {
                neighborhoodManager.UpdateOriginalSolution(bestcandidate);
            }
            return found;
        }
    }

    public class ChooseFirst : ILocalSearchPolicy
    {
        public bool SearchLocally(NeighborhoodManager neighborhoodManager)
        {
            var currentSolution = neighborhoodManager.OriginalSolution;
            while (neighborhoodManager.MoveNext())
            {
                //step 1. get a legal(!) candidate
                SolutionDiff candidate = neighborhoodManager.Current;

                //step 2. add more items to candidate, to make it better, while keeping it legal (Even number of items)
                //this.AddMoreItemsToCandidate(candidate, neighborhoodManager);

                if (currentSolution.WouldBeImprovedBy(candidate))
                {
                    neighborhoodManager.UpdateOriginalSolution(candidate);
                    return true;
                }
            }
            return false; 
            
        }
    }

    static class LocalSearchPolicyExtentionMethods
    {
        /// <summary>
        /// try add more new items to candidate (from unchosen items) till capacity exceeds (while making sure solution stays legal (even) (like in GreedKnapsack)
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="neighborhoodManager"></param>
        public static void AddMoreItemsToCandidate(this ILocalSearchPolicy @this,  SolutionDiff candidate, NeighborhoodManager neighborhoodManager)
        {
            //create an updated copy of UnchosenItems
            var unchosenItems = new HashSet<Item>(neighborhoodManager.UnchosenItems);
            unchosenItems.UnionWith(candidate.RemovedItems);
            unchosenItems.ExceptWith(candidate.AddedItems);
           

            neighborhoodManager.OriginalSolution.AddMoreItemsToCandidate(candidate, unchosenItems);
        }
    }
}