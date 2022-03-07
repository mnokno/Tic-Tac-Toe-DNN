using UnityEngine;
using NN.Training;

namespace TicTacToe.Tests
{
    public static class MergeSort
    {
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
                SortV2(ref candidates);
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

        private static void Sort(ref int[] elements)
        {
            void Merge(ref int[] toSort, int begin, int mid, int end)
            {
                // Extracts the arrays
                int leftSize = mid - begin + 1;
                int righSize = end - mid;

                int[] leftArray = new int[leftSize];
                int[] rightArray = new int[righSize];

                for (int i = 0; i < leftSize; ++i)
                {
                    leftArray[i] = toSort[begin + i];
                }
                for (int i = 0; i < righSize; ++i)
                {
                    rightArray[i] = toSort[mid + 1 + i];
                }


                // Index counters
                int leftIndex = 0;
                int rightIndex = 0;
                int mainIndex = begin;

                // Sorts the result data into the array
                while (leftIndex < leftSize && rightIndex < righSize)
                {
                    if (leftArray[leftIndex] > rightArray[rightIndex])
                    {
                        toSort[mainIndex] = leftArray[leftIndex];
                        leftIndex++;
                    }
                    else
                    {
                        toSort[mainIndex] = rightArray[rightIndex];
                        rightIndex++;
                    }
                    mainIndex++;
                }

                // Appends remaining elements
                while (leftIndex < leftSize)
                {
                    toSort[mainIndex] = leftArray[leftIndex];
                    leftIndex++;
                    mainIndex++;
                }
                while (rightIndex < righSize)
                {
                    toSort[mainIndex] = rightArray[rightIndex];
                    rightIndex++;
                    mainIndex++;
                }
            }

            void MergeSort(ref int[] toSort, int begin, int end)
            {
                if (begin >= end)
                {
                    return;
                }

                int mid = begin + (end - begin) / 2;
                MergeSort(ref toSort, begin, mid);
                MergeSort(ref toSort, mid + 1, end);
                Merge(ref toSort, begin, mid, end);
            }

            MergeSort (ref elements, 0, elements.Length - 1);
        }

        private static void Sort(ref Candidate[] elements)
        {
            void Merge(ref Candidate[] toSort, int begin, int mid, int end)
            {
                // Extracts the arrays
                int leftSize = mid - begin + 1;
                int righSize = end - mid;

                Candidate[] leftArray = new Candidate[leftSize];
                Candidate[] rightArray = new Candidate[righSize];

                for (int i = 0; i < leftSize; ++i)
                {
                    leftArray[i] = toSort[begin + i];
                }
                for (int i = 0; i < righSize; ++i)
                {
                    rightArray[i] = toSort[mid + 1 + i];
                }


                // Index counters
                int leftIndex = 0;
                int rightIndex = 0;
                int mainIndex = begin;

                // Sorts the result data into the array
                while (leftIndex < leftSize && rightIndex < righSize)
                {
                    if (leftArray[leftIndex].score.wins > rightArray[rightIndex].score.wins)
                    {
                        toSort[mainIndex] = leftArray[leftIndex];
                        leftIndex++;
                    }
                    else
                    {
                        toSort[mainIndex] = rightArray[rightIndex];
                        rightIndex++;
                    }
                    mainIndex++;
                }

                // Appends remaining elements
                while (leftIndex < leftSize)
                {
                    toSort[mainIndex] = leftArray[leftIndex];
                    leftIndex++;
                    mainIndex++;
                }
                while (rightIndex < righSize)
                {
                    toSort[mainIndex] = rightArray[rightIndex];
                    rightIndex++;
                    mainIndex++;
                }
            }

            void MergeSort(ref Candidate[] toSort, int begin, int end)
            {
                if (begin >= end)
                {
                    return;
                }

                int mid = begin + (end - begin) / 2;
                MergeSort(ref toSort, begin, mid);
                MergeSort(ref toSort, mid + 1, end);
                Merge(ref toSort, begin, mid, end);
            }

            MergeSort(ref elements, 0, elements.Length - 1);
        }

        private static void SortV2(ref Candidate[] elements)
        {
            // Creates a local version of the elements list
            Candidate[] lElements = elements;

            void Merge(ref int[] toSort, int begin, int mid, int end)
            {
                // Extracts the arrays
                int leftSize = mid - begin + 1;
                int righSize = end - mid;

                int[] leftArray = new int[leftSize];
                int[] rightArray = new int[righSize];

                for (int i = 0; i < leftSize; ++i)
                {
                    leftArray[i] = toSort[begin + i];
                }
                for (int i = 0; i < righSize; ++i)
                {
                    rightArray[i] = toSort[mid + 1 + i];
                }


                // Index counters
                int leftIndex = 0;
                int rightIndex = 0;
                int mainIndex = begin;

                // Sorts the result data into the array
                while (leftIndex < leftSize && rightIndex < righSize)
                {
                    if (lElements[leftArray[leftIndex]].score.wins > lElements[rightArray[rightIndex]].score.wins)
                    {
                        toSort[mainIndex] = leftArray[leftIndex];
                        leftIndex++;
                    }
                    else
                    {
                        toSort[mainIndex] = rightArray[rightIndex];
                        rightIndex++;
                    }
                    mainIndex++;
                }

                // Appends remaining elements
                while (leftIndex < leftSize)
                {
                    toSort[mainIndex] = leftArray[leftIndex];
                    leftIndex++;
                    mainIndex++;
                }
                while (rightIndex < righSize)
                {
                    toSort[mainIndex] = rightArray[rightIndex];
                    rightIndex++;
                    mainIndex++;
                }
            }

            void MergeSort(ref int[] toSort, int begin, int end)
            {
                if (begin >= end)
                {
                    return;
                }

                int mid = begin + (end - begin) / 2;
                MergeSort(ref toSort, begin, mid);
                MergeSort(ref toSort, mid + 1, end);
                Merge(ref toSort, begin, mid, end);
            }

            // Optimization
            int[] indexes = new int[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                indexes[i] = i;
            }

            // Sorts the list by manipulating indexes of the elements (not the actual element)
            MergeSort(ref indexes, 0, elements.Length - 1);

            // Converts the sorted index list to a sorted list of candidates
            Candidate[] sorted = new Candidate[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                sorted[i] = elements[indexes[i]];
            }

            // Assignees sorted list to the reference list
            elements = sorted;
        }
    }
}
