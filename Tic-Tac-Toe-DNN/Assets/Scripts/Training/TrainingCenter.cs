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
        /// Runs training session on the parents array, returns an array of numRes best parents, 80% parent based nets, 20% random to avoid getting stack in local minimums
        /// </summary>
        public static Candidate[] RunTrainingSession(Candidate[] parrents, int numCandidates, int numSubsets, int numRes)
        {
            // Candidates an empty array for candidates
            Candidate[] candidates = new Candidate[numCandidates];

            // Calculates split for random and parent based children
            int perParrent = Mathf.FloorToInt(numCandidates * 0.8f / parrents.Length);
            int perParrentReshuffle = Mathf.FloorToInt(numCandidates * 0.1f / parrents.Length);
            int random = numCandidates - (perParrent + perParrentReshuffle) * parrents.Length;

            // Returns the best candidates
            return SelectBest(candidates, numSubsets, numRes);
        }

        /// <summary>
        /// Runs training session on a new population, returns an array of numRes best parents
        /// </summary>
        public static Candidate[] RunTrainingSession(int numCandidates, int numSubsets, int numRes, int numInput, int[] numHidden, int numOutput)
        {
            return SelectBest(GetRandomCandidates(numCandidates, numInput, numHidden, numOutput), numSubsets, numRes);
        }

        /// <summary>
        /// Selects best networks from the candidates array by simulating games
        /// </summary>
        public static Candidate[] SelectBest(Candidate[] candidates, int numSubsets, int numRes)
        {
            return null;
        }

        /// <summary>
        /// Generates an array of candidates with random brains CLI
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
