using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NN.Training;

namespace TicTacToe.Tests
{
    public static class DataGenerator 
    {
        /// <summary>
        /// Generates an array of random integers
        /// </summary>
        public static int[] GenerateRandomIntsArray(int numElements)
        {
            // Creates the array
            int[] numbers = new int[numElements];

            // Fills the array with random numbers
            System.Random random = new System.Random();
            for (int i = 0; i < numElements; i++)
            {
                numbers[i] = random.Next(-1000, 1000);
            }

            // Return the list;
            return numbers;
        }

        /// <summary>
        /// Generates an array of random candidates
        /// </summary>
        /// <param name="numElements"></param>
        /// <returns></returns>
        public static Candidate[] GenerateRandomCandidatesArray(int numElements)
        {
            // Creates the array
            Candidate[] candidates = TrainingCenter.GetRandomCandidates(numElements, 10, new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 }, 1);

            // Fills the array with random numbers
            System.Random random = new System.Random();
            for (int i = 0; i < numElements; i++)
            {
                candidates[i].score.wins = random.Next(-1000, 1000);
                candidates[i].score.loses = random.Next(-1000, 1000);
                candidates[i].score.draws = random.Next(-1000, 1000);
            }

            // Return the list;
            return candidates;
        }
    }
}
