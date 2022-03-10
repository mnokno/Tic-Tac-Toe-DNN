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
        //private int numInput = 9;
        //private int[] numHidden = new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 };
        //private int numOutput = 1;
        private int numInput = 9;
        private int[] numHidden = new int[] { 36, 36};
        private int numOutput = 9;
        // Flag used to decided what training should be run
        public bool newNetwork = false;

        /// <summary>
        /// Runs training session on the parents array, returns an array of numRes best parents, 80% parent based nets, 20% random to avoid getting stack in local minimums
        /// </summary>
        public void RunTrainingSession(int numParrents, int numCandidates, int numRes)
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
                    try
                    {
                        EvolutionaryNeuralNetwork evolutionaryNeuralNetwork = EvolutionaryNeuralNetwork.Coppy(c.AI.brain);
                        evolutionaryNeuralNetwork.Mutate(0.001f, 0.2f);
                        Candidate candidate = new Candidate(evolutionaryNeuralNetwork);
                        candidates[nextCandidateIndex] = candidate;
                        nextCandidateIndex++;
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                for (int i = 0; i < perParrentReshuffle; i++)
                {
                    candidates[nextCandidateIndex] = new Candidate(EvolutionaryNeuralNetwork.Coppy(c.AI.brain));
                    candidates[nextCandidateIndex].AI.brain.Mutate(0.01f, 1f);
                    nextCandidateIndex++;
                }
            }
            for (int j = 0; j < random; j++)
            {
                candidates[nextCandidateIndex] = GetRandomCandidate(numInput, numHidden, numOutput);
                nextCandidateIndex++;
            }

            // Returns the best candidates
            SelectBest(candidates, numRes);
        }

        /// <summary>
        /// Runs training session on a new population, returns an array of numRes best parents
        /// </summary>
        public void RunTrainingSession(int numCandidates, int numRes)
        {
            SelectBest(GetRandomCandidates(numCandidates, numInput, numHidden, numOutput), numRes);
        }

        /// <summary>
        /// Selects best networks from the candidates array by simulating games
        /// </summary>
        public void SelectBest(Candidate[] candidates, int numRes)
        {
            // Calculates total number of matches to be player
            taskProgress.totalGamesToSimulate += candidates.Length * (candidates.Length - 1) / 2;
            taskProgress.startedTraining = true;

            // Simulates games in main set
            for (int x = 0; x < candidates.Length - 1; x++)
            {
                for (int y = x + 1; y < candidates.Length; y++)
                {
                    GameSimulator.SimulateMatch(ref candidates[x], ref candidates[y]);
                    taskProgress.simulatedGames++;
                }
            }

            // Sorts main set
            Sort(ref candidates);

            // Returns best candidates from the main set
            Candidate[] topCandidates = new Candidate[numRes];
            for (int i = 0; i < numRes; i++)
            {
                topCandidates[i] = candidates[i];
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
