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
    }
}
