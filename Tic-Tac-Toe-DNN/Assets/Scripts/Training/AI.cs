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
            double bestScore = double.MaxValue;
            foreach (SquarePos move in moves)
            {
                board.MakeMove(move, false);
                double score = brain.ComputeOutputs(FormatDataForDNN(board), false)[0];
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                board.UnMakeMove();
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
            double[] data = new double[board.dimensions * board.dimensions + 1];

            // Adds side to move
            data[0] = (int)board.sideToMove;

            // Adds squares data
            for (int x = 0; x < board.dimensions; x++)
            {
                for (int y = 0; y < board.dimensions; y++)
                {
                    data[1 + x * 3 + y] = (int)board.GetFieldType(x, y);
                }
            }

            // Returns formated data
            return data;
        }
    }
}