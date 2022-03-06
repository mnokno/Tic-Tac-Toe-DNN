using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN.Training
{
    public class TrainingCenter
    {
        public void Test()
        {
            Candidate[] candidates = GetRandomCandidates(2, 10, new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 }, 1);
            GameSimulator.SimulateMatch(ref candidates[0], ref candidates[1]);
            Debug.Log(candidates[0].score);
            Debug.Log(candidates[1].score);
            GameSimulator.SimulateMatch(ref candidates[0], ref candidates[1]);
            GameSimulator.SimulateMatch(ref candidates[0], ref candidates[1]);
            Debug.Log(candidates[0].score);
            Debug.Log(candidates[1].score);
        }

        /// <summary>
        /// Generates an array of candidates with random brains
        /// </summary>
        public static Candidate[] GetRandomCandidates(int numCandidates, int numInput, int[] numHidden, int numOutput)
        {
            Candidate[] candidates = new Candidate[numCandidates];

            for (int i = 0; i < numCandidates; i++)
            {
                candidates[i] = new Candidate(new EvolutionaryNeuralNetwork(numInput, numHidden, numOutput));
            }

            return candidates;
        }
    }
}
