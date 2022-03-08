using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Tests
{
    public static class Test 
    {
        public static void Run(int testNumber)
        {
            switch (testNumber)
            {
                case 1:
                    Test1();
                    break;
                case 2:
                    Test2();
                    break;
                case 3:
                    Test3();
                    break;
                case 4:
                    Test4();
                    break;
            }
        }

        private static void Test1()
        {
            long total = 0;
            int batchSize = 1000;
            for (int i = 0; i < batchSize; i++)
            {
                total += MergeSort.TimeSort(100, sortCandidates: true);
            }
            Debug.Log(total / batchSize);
        }

        private static void Test2()
        {
            long total = 0;
            int batchSize = 100;
            for (int i = 0; i < batchSize; i++)
            {
                total += MergeSort.TimeSort(1000);
            }
            Debug.Log(total / batchSize);
        }

        private static void Test3()
        {
            long total = 0;
            int batchSize = 1000;
            for (int i = 0; i < batchSize; i++)
            {
                total += QuickSort.TimeSort(100, sortCandidates: true);
            }
            Debug.Log(total / batchSize);
        }

        private static void Test4()
        {
            long total = 0;
            int batchSize = 100;
            for (int i = 0; i < batchSize; i++)
            {
                total += QuickSort.TimeSort(1000);
            }
            Debug.Log(total / batchSize);
        }
    }
}
