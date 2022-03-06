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
        public static void SimulateMatch(ref Candidate candidateA, ref Candidate candidateB)
        {
            SimulateGame(ref candidateA, ref candidateB);
            SimulateGame(ref candidateB, ref candidateA);
        }

        /// <summary>
        /// Simulates a game, candidateA will play as x and candidateB as o, scores will be automatically updated
        /// </summary>
        public static void SimulateGame(ref Candidate candidateA, ref Candidate candidateB)
        {
            // Creates a new game
            Board board = new Board(3, 3, Board.GameMode.line);

            // Simulates the game
            while (board.gameState == Board.GameState.onGoing)
            {
                if (board.sideToMove == Board.SideToMove.x)
                {
                    board.MakeMove(candidateA.AI.ChouseMove(board), false);
                }
                else
                {
                    board.MakeMove(candidateB.AI.ChouseMove(board), false);
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
    }
}
