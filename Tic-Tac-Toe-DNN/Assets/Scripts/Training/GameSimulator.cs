using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Technical;

namespace NN.Training
{
    public static class GameSimulator
    {
        /// <summary>
        /// Simulates a match consisting of two game, each played will get to play as x and o, scores will be automatically updated
        /// </summary>
        public static void SimulateMatch(Candidate candidateA, Candidate candidateB)
        {
            SimulateGame(candidateA, candidateB);
            SimulateGame(candidateB, candidateA);
        }

        /// <summary>
        /// Simulates a game, candidateA will play as x and candidateB as o, scores will be automatically updated
        /// </summary>
        public static void SimulateGame(Candidate candidateA, Candidate candidateB)
        {
            // Creates a new game
            Board board = new Board(3, 3, Board.GameMode.line);

            // Simulates the game
            while (board.gameState == Board.GameState.onGoing)
            {
                if (board.sideToMove == Board.SideToMove.x)
                {
                    board.MakeMove(ChouseMove(candidateA, board), false);
                }
                else
                {
                    board.MakeMove(ChouseMove(candidateB, board), false);
                }
            }

            // Updates scores
            if (board.gameState == Board.GameState.draw)
            {
                candidateA.score.draws++;
                candidateB.score.draws++;
            }
            else if (board.gameState == Board.GameState.xWon)
            {
                candidateA.score.wins++;
                candidateB.score.loses++;
            }
            else // oWon
            {
                candidateA.score.loses++;
                candidateB.score.wins++;
            }
        }

        /// <summary>
        /// Returns move chosen by the candidate on the given board
        /// </summary>
        private static SquarePos ChouseMove(Candidate candidate, Board board)
        {
            // Gets a list of all legal moves
            SquarePos[] moves = board.GenerateLegalMoves();

            // Finds the best move using the candidates brain
            SquarePos bestMove = moves[0];
            double bestScore = double.MaxValue;
            foreach (SquarePos move in moves)
            {
                board.MakeMove(move, false);
                double score = candidate.brain.ComputeOutputs(FormatDataForDNN(board), false)[0];
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
