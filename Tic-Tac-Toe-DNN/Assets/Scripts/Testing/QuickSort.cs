using UnityEngine;
using NN.Training;

namespace TicTacToe.Tests
{
    public static class QuickSort
    {
        /// <summary>
        /// Times how long it takes to perform a quick sort, return the time taken in ticks
        /// </summary>
        public static long TimeSort(int numElements, bool sortCandidates = false, bool logSorted = false)
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
            void QuickSort(ref int[] toSort, int beg, int end)
            {
                if (beg < end)
                {
                    // Orders the partition and return the index of the pivot point after ordering 
                    int pivotIndex = Partition(ref toSort, beg, end);

                    // Sorts the partition to the left and right of the pivot index
                    QuickSort(ref toSort, beg, pivotIndex - 1);
                    QuickSort(ref toSort, pivotIndex + 1, end);
                }
            }

            int Partition(ref int[] toSort, int beg, int end)
            {
                // Gets the pivot element (most right element)
                int pivot = toSort[end];

                // Orders the partition
                int pivotIndex = beg;
                for (int j = beg; j < end; j++)
                {
                    if (toSort[j] > pivot)
                    {
                        (toSort[pivotIndex], toSort[j]) = (toSort[j], toSort[pivotIndex]);
                        pivotIndex++;
                    }
                }
                (toSort[pivotIndex], toSort[end]) = (toSort[end], toSort[pivotIndex]);

                // Return the pivot index
                return pivotIndex;
            }

            QuickSort(ref elements, 0, elements.Length - 1);
        }

        /// <summary>
        /// Sorts the array of candidates using quick sort
        /// </summary>
        private static void Sort(ref Candidate[] elements)
        {

        }
    }
}
