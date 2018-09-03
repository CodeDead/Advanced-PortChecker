using System.Collections.Generic;

namespace Advanced_PortChecker.Classes.Utils
{
    /// <summary>
    /// Internal class to calculate the amount of actions that can to be performed per thread
    /// </summary>
    internal static class ThreadCalculator
    {
        /// <summary>
        /// Get the actions that can be performed per thread
        /// </summary>
        /// <param name="numberOfThreads">The number of threads that need to perform a certain amount of actions</param>
        /// <param name="numberOfActions">The number of actions that need to be performed by the threads</param>
        /// <returns>The number of actions that need to be performed per thread</returns>
        internal static IEnumerable<int> GetActionsPerThreads(int numberOfThreads, int numberOfActions)
        {
            // We have too many threads for the requested amount of actions
            while (numberOfActions < numberOfThreads)
            {
                numberOfThreads--;
            }

            int[] actionsPerThread = new int[numberOfThreads];
            // Intentional loss of data
            int perThread = numberOfActions / numberOfThreads;
            int actionRemainder = numberOfActions - (numberOfThreads * perThread);

            // No need to split anything. The threads can perform an equal amount of actions
            if (actionRemainder == 0)
            {
                for (int i = 0; i < numberOfThreads; i++)
                {
                    actionsPerThread[i] = perThread;
                }
            }
            // We have more actions than we have threads. Time to reduce our thread count to the amount of actions
            else if (numberOfThreads > numberOfActions)
            {
                for (int i = 0; i < numberOfActions; i++)
                {
                    actionsPerThread[i]++;
                }
            }
            // We have an unequal amount of actions per thread, time to split them
            else
            {
                // All threads perform the calculated amount of actions (rounded down)
                for (int i = 0; i < actionsPerThread.Length; i++)
                {
                    actionsPerThread[i] = perThread;
                }

                // Some tasks will have to do more work
                for (int i = 0; i < actionRemainder; i++)
                {
                    actionsPerThread[i]++;
                }
            }
            return actionsPerThread;
        }
    }
}
