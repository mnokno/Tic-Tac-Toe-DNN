using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Technical;

namespace NN.Training
{
    public struct AI
    {
        public EvolutionaryNeuralNetwork brain;
        public AI(EvolutionaryNeuralNetwork brain)
        {
            this.brain = brain;
        }

        /// <summary>
        /// Returns move chosen on the given board
        /// </summary>
        public SquarePos ChouseMove(Board board)
        {
            // Gets a list of all legal moves
            SquarePos[] moves = board.GenerateLegalMoves();

            // Finds the best move using the candidates brain
            SquarePos bestMove = moves[0];
            int[] indexes = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            double[] results = brain.ComputeOutputs(FormatDataForDNN(board), true);

            //double bestScore = double.MaxValue;
            //foreach (SquarePos move in moves)
            //{
            //    board.MakeMove(move, false);
            //    double score = brain.ComputeOutputs(FormatDataForDNN(board), false)[0];
            //    if (score < bestScore)
            //    {
            //        bestScore = score;
            //        bestMove = move;
            //    }
            //   board.UnMakeMove();
            //}
            Sort(ref indexes, results);
            for (int i = 0; i < indexes.Length; i++)
            {
                SquarePos move = SquarePos.IndexToSquarePos(indexes[i], 3);
                if (board.IsEmpty(move))
                {
                    return move;
                }
            }

            // Returns the best move
            return bestMove;
        }

        /// <summary>
        /// Extracts required information from a board and formats in as an array for DNN processing
        /// </summary>
        private static double[] FormatDataForDNN(Board board)
        {
            // Creates an empty array
            double[] data = new double[board.dimensions * board.dimensions];

            // Find side to move multiplier
            int mult = board.sideToMove == Board.SideToMove.x ? 1 : -1;

            // Adds squares data
            for (int x = 0; x < board.dimensions; x++)
            {
                for (int y = 0; y < board.dimensions; y++)
                {
                    data[x * 3 + y] = (int)board.GetFieldType(x, y) * mult;
                }
            }

            // Returns formated data
            return data;
        }

        /// <summary>
        /// Sorts the array of integers using quick sort
        /// </summary>
        private static void Sort(ref int[] elements, double[] data)
        {
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
                    if (data[toSort[j]] > data[pivot])
                    {
                        (toSort[pivotIndex], toSort[j]) = (toSort[j], toSort[pivotIndex]);
                        pivotIndex++;
                    }
                }
                (toSort[pivotIndex], toSort[end]) = (toSort[end], toSort[pivotIndex]);

                // Return the pivot index
                return pivotIndex;
            }

            QuickSort(ref elements, 0, elements.Length - 1);
        }
    }
}