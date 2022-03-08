using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Tests
{
    public static class Test 
    {
        public static void Run(int testNumber, int batches, int batchSize)
        {
            switch (testNumber)
            {
                case 1:
                    Test1(batches, batchSize);
                    break;
                case 2:
                    Test2(batches, batchSize);
                    break;
                case 3:
                    Test3(batches, batchSize);
                    break;
                case 4:
                    Test4(batches, batchSize);
                    break;
            }
        }

        private static void Test1(int batches, int batchSize)
        {
            long total = 0;
            for (int i = 0; i < batches; i++)
            {
                total += MergeSort.TimeSort(batchSize, sortCandidates: true);
            }
            Debug.Log($"MergeSort: candidates, Ticks: {total / batches}, Batches: {batches}, BatchSize: {batchSize}");
        }

        private static void Test2(int batches, int batchSize)
        {
            long total = 0;
            for (int i = 0; i < batches; i++)
            {
                total += MergeSort.TimeSort(batchSize);
            }
            Debug.Log($"MergeSort: integers, Ticks: {total / batches}, Batches: {batches}, BatchSize: {batchSize}");
        }

        private static void Test3(int batches, int batchSize)
        {
            long total = 0;
            for (int i = 0; i < batches; i++)
            {
                total += QuickSort.TimeSort(batchSize, sortCandidates: true);
            }
            Debug.Log($"QuickSort: candidates, Ticks: {total / batches}, Batches: {batches}, BatchSize: {batchSize}");
        }

        private static void Test4(int batches, int batchSize)
        {
            long total = 0;
            for (int i = 0; i < batches; i++)
            {
                total += QuickSort.TimeSort(batchSize);
            }
            Debug.Log($"QuickSort: integers, Ticks: {total / batches}, Batches: {batches}, BatchSize: {batchSize}");
        }
    }
}
