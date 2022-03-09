using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NN.Training
{
    public class TrainingCenter
    {
        // Used to expose training progress
        public TaskProgress taskProgress = new TaskProgress();
        // Stores best candidates fro previous training session
        public Stack<Candidate[]> bestCandidates = new Stack<Candidate[]>();
        // Network topology
        private int numInput = 10;
        private int[] numHidden = new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 };
        private int numOutput = 1;
        // Flag used to decided what training should be run
        public bool newNetwork = false;

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
        public void RunTrainingSession(int numParrents, int numCandidates, int numSubsets, int mainsetSize, int numRes)
        {
            // Candidates an empty array for candidates
            Candidate[] candidates = new Candidate[numCandidates];

            // Caps number of parents
            numParrents = numParrents < bestCandidates.Peek().Length ? bestCandidates.Peek().Length : numParrents;

            // Calculates split for random and parent based children
            int perParrent = Mathf.FloorToInt(numCandidates * 0.8f / numParrents);
            int perParrentReshuffle = Mathf.FloorToInt(numCandidates * 0.1f / numParrents);
            int random = numCandidates - (perParrent + perParrentReshuffle) * numParrents;

            // Generates the candidates
            int nextCandidateIndex = 0;
            Candidate[] parrent = bestCandidates.Peek();
            foreach (Candidate c in parrent)
            {
                for (int i = 0; i < perParrent; i++)
                {
                    candidates[nextCandidateIndex] = new Candidate(EvolutionaryNeuralNetwork.Coppy(c.AI.brain));
                    candidates[nextCandidateIndex].AI.brain.Mutate(0.05f, 0.2f);
                    nextCandidateIndex++;
                }
                for (int i = 0; i < perParrentReshuffle; i++)
                {
                    candidates[nextCandidateIndex] = new Candidate(EvolutionaryNeuralNetwork.Coppy(c.AI.brain));
                    candidates[nextCandidateIndex].AI.brain.Mutate(0.1f, 2f);
                    nextCandidateIndex++;
                }
            }
            for (int j = 0; j < random; j++)
            {
                candidates[nextCandidateIndex] = GetRandomCandidate(numInput, numHidden, numOutput);
                nextCandidateIndex++;
            }

            // Returns the best candidates
            SelectBest(candidates, numSubsets, mainsetSize, numRes);
        }

        /// <summary>
        /// Runs training session on a new population, returns an array of numRes best parents
        /// </summary>
        public void RunTrainingSession(int numCandidates, int numSubsets, int mainsetSize, int numRes)
        {
            SelectBest(GetRandomCandidates(numCandidates, numInput, numHidden, numOutput), numSubsets, mainsetSize, numRes);
        }

        /// <summary>
        /// Selects best networks from the candidates array by simulating games
        /// </summary>
        public void SelectBest(Candidate[] candidates, int numSubsets, int mainsetSize, int numRes)
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
            taskProgress.totalGamesToSimulate += mainsetSize * (mainsetSize - 1) / 2;
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

            // Sorts each subset
            for (int i = 0; i < subsets.Length; i++)
            {
                Sort(ref subsets[i]);
            }

            // Creates the main set
            Candidate[] mainSet = new Candidate[mainsetSize];
            int currentMainSetIndex = 0;
            for (int i = 0; i < fromSubsetsToMainset.Length; i++)
            {
                for (int j = 0; j < fromSubsetsToMainset[i]; j++)
                {
                    mainSet[currentMainSetIndex] = subsets[i][j];
                    mainSet[currentMainSetIndex].score.Reset();
                    currentMainSetIndex++;
                }
            }

            // Simulates games in main set
            for (int x = 0; x < mainSet.Length - 1; x++)
            {
                for (int y = x + 1; y < mainSet.Length; y++)
                {
                    GameSimulator.SimulateMatch(ref mainSet[x], ref mainSet[y]);
                    taskProgress.simulatedGames++;
                }
            }

            // Sorts main set
            Sort(ref mainSet);

            // Returns best candidates from the main set
            Candidate[] topCandidates = new Candidate[numRes];
            for (int i = 0; i < numRes; i++)
            {
                topCandidates[i] = mainSet[i];
            }

            // Adds top candidates to the results stack
            bestCandidates.Push(topCandidates);
        }

        /// <summary>
        /// Generates an array of candidates with random brains CLI
        /// </summary>
        public static Candidate[] GetRandomCandidates(int numCandidates, int numInput, int[] numHidden, int numOutput)
        {
            Candidate[] candidates = new Candidate[numCandidates];

            for (int i = 0; i < numCandidates; i++)
            {
                candidates[i] = GetRandomCandidate(numInput, numHidden, numOutput);
            }

            return candidates;
        }

        /// <summary>
        /// Generates a candidate with random brains CLI
        /// </summary>
        public static Candidate GetRandomCandidate(int numInput, int[] numHidden, int numOutput)
        {
            return new Candidate(new EvolutionaryNeuralNetwork(numInput, numHidden, numOutput));
        }

        /// <summary>
        /// Sorts the array of candidates using quick sort
        /// </summary>
        private static void Sort(ref Candidate[] elements)
        {
            // Creates a local version of the elements list
            Candidate[] lElements = elements;

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
                    if (lElements[toSort[j]].score > lElements[pivot].score)
                    {
                        (toSort[pivotIndex], toSort[j]) = (toSort[j], toSort[pivotIndex]);
                        pivotIndex++;
                    }
                }
                (toSort[pivotIndex], toSort[end]) = (toSort[end], toSort[pivotIndex]);

                // Return the pivot index
                return pivotIndex;
            }

            // Optimization
            int[] indexes = new int[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                indexes[i] = i;
            }

            // Sorts the list by manipulating indexes of the elements (not the actual element)
            QuickSort(ref indexes, 0, elements.Length - 1);

            // Converts the sorted index list to a sorted list of candidates
            Candidate[] sorted = new Candidate[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                sorted[i] = elements[indexes[i]];
            }

            // Assignees sorted list to the reference list
            elements = sorted;
        }

        /// <summary>
        /// Used to show training progress
        /// </summary>
        public struct TaskProgress
        {
            public int totalGamesToSimulate;
            public int simulatedGames;
            public bool startedTraining;

            public void Reset()
            {
                totalGamesToSimulate = 0;
                simulatedGames = 0;
                startedTraining = false;
            }
        }
    }
}
