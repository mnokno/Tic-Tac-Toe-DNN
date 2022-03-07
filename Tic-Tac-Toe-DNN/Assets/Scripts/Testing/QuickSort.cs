using UnityEngine;
using NN.Training;

namespace TicTacToe.Tests
{
    public static class QuickSort
    {
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
                // Optionally print the sorted list
                if (logSorted)
                {
                    string formated = "";
                    foreach (Candidate c in candidates)
                    {
                        formated += c.score;
                    }
                    Debug.Log(formated);
                }
                // Stops the stopwatch
                stopwatch.Stop();
            }
            else
            {
                // Creates the list of elements to sort
                int[] numbers = DataGenerator.GenerateRandomIntsArray(numElements);
                // Start the stopwatch
                stopwatch.Start();
                // Sorts the list
                Sort(ref numbers);
                // Optionally print the sorted list
                if (logSorted)
                {
                    Debug.Log(string.Join("\n", numbers));
                }
                // Stops the stopwatch
                stopwatch.Stop();
            }

            // Returns the time taken to sort the list
            return stopwatch.ElapsedMilliseconds;
        }

        private static float Sort(ref int[] elements)
        {
            return 0;
        }

        private static float Sort(ref Candidate[] elements)
        {
            return 0;
        }
    }
}
