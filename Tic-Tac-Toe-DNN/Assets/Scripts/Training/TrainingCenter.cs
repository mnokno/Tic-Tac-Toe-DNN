using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NN.Training
{
    public class TrainingCenter
    {
        public TaskProgress taskProgress = new TaskProgress();

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
        public Candidate[] RunTrainingSession(Candidate[] parrents, int numCandidates, int numSubsets, int mainsetSize, int numRes)
        {
            // Candidates an empty array for candidates
            Candidate[] candidates = new Candidate[numCandidates];

            // Calculates split for random and parent based children
            int perParrent = Mathf.FloorToInt(numCandidates * 0.8f / parrents.Length);
            int perParrentReshuffle = Mathf.FloorToInt(numCandidates * 0.1f / parrents.Length);
            int random = numCandidates - (perParrent + perParrentReshuffle) * parrents.Length;

            // Returns the best candidates
            return SelectBest(candidates, numSubsets, mainsetSize, numRes);
        }

        /// <summary>
        /// Runs training session on a new population, returns an array of numRes best parents
        /// </summary>
        public Candidate[] RunTrainingSession(int numCandidates, int numSubsets, int mainsetSize, int numRes, int numInput, int[] numHidden, int numOutput)
        {
            return SelectBest(GetRandomCandidates(numCandidates, numInput, numHidden, numOutput), numSubsets, mainsetSize, numRes);
        }

        /// <summary>
        /// Selects best networks from the candidates array by simulating games
        /// </summary>
        public Candidate[] SelectBest(Candidate[] candidates, int numSubsets, int mainsetSize, int numRes)
        {
            // Shuffles the list of candidates
            System.Random random = new System.Random();
            candidates = candidates.OrderBy(x => random.Next()).ToArray();

            // Calculates number of candidates per subset
            int subsetSize = candidates.Length / numSubsets;
            int[] subsetSizes = new int[numSubsets];
            for (int i = 0; i < numSubsets - 1; i++)
            {
                subsetSizes[i] = subsetSize;
            }
            subsetSizes[numSubsets - 1] = candidates.Length - subsetSize * (numSubsets - 1);

            // Calculates total number of matches to be player
            foreach (int ss in subsetSizes)
            {
                taskProgress.totalGamesToSimulate += ss * (ss - 1) / 2;
            }
            taskProgress.startedTraining = true;

            // Creates the subsets
            Candidate[][] subsets = new Candidate[numSubsets][];
            int rolingTotal = 0;
            for (int i = 0; i < numSubsets; i++)
            {
                subsets[i] = new Candidate[subsetSizes[i]];
                System.Array.Copy(candidates, rolingTotal, subsets[i], 0, subsetSizes[i]);
                rolingTotal += subsetSizes[i];
            }

            // Simulates the tournament for each subset
            for (int i = 0; i < numSubsets; i++)
            {
                for (int x = 0; x < subsets[i].Length - 1; x++)
                {
                    for (int y = x + 1; y < subsets[i].Length; y++)
                    {
                        GameSimulator.SimulateMatch(ref subsets[i][x], ref subsets[i][y]);
                        taskProgress.simulatedGames++;
                    }
                }
            }

            // Calculates how many candidates should move on to main set
            int[] fromSubsetsToMainset = new int[numSubsets];
            int subsetProgressionCount = mainsetSize / numSubsets;
            for (int i = 0; i < numSubsets - 1; i++)
            {
                fromSubsetsToMainset[i] = subsetProgressionCount;
            }
            fromSubsetsToMainset[numSubsets - 1] = mainsetSize - subsetProgressionCount * (numSubsets - 1);

            Debug.Log(string.Join(" ", fromSubsetsToMainset));
            Debug.Log(subsets[1][23].score);
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

        /// <summary>
        /// Used to show training progress
        /// </summary>
        public struct TaskProgress
        {
            public int totalGamesToSimulate;
            public int simulatedGames;
            public bool startedTraining;
        }
    }
}
