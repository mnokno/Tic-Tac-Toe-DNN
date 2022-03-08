using UnityEngine;
using NN.Training;

namespace TicTacToe.Tests
{
    public static class QuickSort
    {
        /// <summary>
        /// Times how long it takes to perform a quick sort, return the time taken in ticks
        /// </summary>
        public static float TimeSort(int numElements, bool sortCandidates = false, bool logSorted = false)
        {
            // Creates the stopwatch
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            if (sortCandidates)
            {
                // Creates the list of elements to sort
                Candidate[] candidates = DataGenerator.GenerateRandomCandidatesArray(numElements);
                // Start the stopwatch
                stopwatch.Start();
                // Sorts the list
                Sort(ref candidates);
                // Stops the stopwatch
                stopwatch.Stop();
                // Optionally print the sorted list
                if (logSorted)
                {
                    string formated = "";
                    foreach (Candidate c in candidates)
                    {
                        formated += "\n" + c.score;
                    }
                    Debug.Log(formated);
                }
            }
            else
            {
                // Creates the list of elements to sort
                int[] numbers = DataGenerator.GenerateRandomIntsArray(numElements);
                // Start the stopwatch
                stopwatch.Start();
                // Sorts the list
                Sort(ref numbers);
                // Stops the stopwatch
                stopwatch.Stop();
                // Optionally print the sorted list
                if (logSorted)
                {
                    Debug.Log(string.Join("\n", numbers));
                }
            }

            // Returns the time taken to sort the list
            return stopwatch.ElapsedTicks;
        }

        /// <summary>
        /// Sorts the array of integers using quick sort
        /// </summary>
        private static void Sort(ref int[] elements)
        {

        }

        /// <summary>
        /// Sorts the array of candidates using quick sort
        /// </summary>
        private static void Sort(ref Candidate[] elements)
        {

        }
    }
}
