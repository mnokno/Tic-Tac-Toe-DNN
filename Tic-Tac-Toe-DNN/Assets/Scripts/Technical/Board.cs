using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Technical
{
    public class Board
    {
        public int dimensions { private set; get; }
        public GameState gameState { private set; get; }
        public SideToMove sideToMove { private set; get; }
        public int lineLenght { private set; get; }
        public GameMode gameMode { private set; get; }
        private Field[,] fields;
        private Stack<SquarePos> history = new Stack<SquarePos>();
        private static readonly SquarePos[] rayLines = new SquarePos[] { new SquarePos(1, 0), new SquarePos(0, 1), new SquarePos(1, -1), new SquarePos(1, 1) };
        private static readonly SquarePos[] rayDirections = new SquarePos[] { new SquarePos(1, 0), new SquarePos(-1, 0), new SquarePos(0, 1), new SquarePos(0, -1), new SquarePos(1, -1), new SquarePos(-1, 1), new SquarePos(1, 1), new SquarePos(-1, -1) };
        private int[][,] squaresToEdge = new int[8][,];

        public Board(int dimensions, int lineLenght, GameMode gameMode)
        {
            this.dimensions = dimensions;
            this.lineLenght = lineLenght;
            this.gameMode = gameMode;
            gameState = GameState.onGoing;
            sideToMove = SideToMove.x;
            InitFields();
        }

        /// <summary>
        /// Initiates fields 2D array
        /// </summary>
        private void InitFields()
        {
            fields = new Field[dimensions, dimensions];
            for (int x = 0; x < dimensions; x++)
            {
                for (int y = 0; y < dimensions; y++)
                {
                    fields[x, y] = Field.empty;
                }
            }
        }

        /// <summary>
        /// Calculates the squares to edge table
        /// </summary>
        private void CalculateSquaresToEdge()
        {
            bool IsValid(SquarePos squarePos)
            {
                return true;
            }

            for (int i = 0; i < 8; i++)
            {
                squaresToEdge[i] = new int[dimensions, dimensions];
            }

            for (int x = 0; x < dimensions; x++)
            {
                for (int y = 0; y < dimensions; y++)
                {
                    for (int i = 0; i < rayDirections.Length; i++)
                    {
                        int toEdge = 0;
                        SquarePos currentPosition = new SquarePos(x, y);
                        while (IsValid(currentPosition))
                        {
                            toEdge++;
                        }
                        squaresToEdge[i][x, y] = toEdge;
                    }
                }
            }
        }

        /// <summary>
        /// Makes given move on the board
        /// </summary>
        public void MakeMove(SquarePos move, bool validate)
        {
            if (gameState == GameState.onGoing)
            {
                if (validate)
                {
                    if (fields[move.x, move.y] != Field.empty)
                    {
                        return;
                    }
                }

                history.Push(move);
                fields[move.x, move.y] = sideToMove == SideToMove.x ? Field.x : Field.o;
                sideToMove = sideToMove == SideToMove.x ? SideToMove.o : SideToMove.x;

                UpdateGameState(move);
            }
        }

        /// <summary>
        /// Unmakes previously made move
        /// </summary>
        public void UnMakeMove()
        {
            if (history.Count != 0)
            {
                SquarePos move = history.Pop();
                fields[move.x, move.y] = Field.empty;
                sideToMove = sideToMove == SideToMove.x ? SideToMove.o : SideToMove.x;
                gameState = GameState.onGoing;
            }
        }

        /// <summary>
        /// Updates game state after a move is made
        /// </summary>
        private void UpdateGameState(SquarePos move)
        {
            if (gameMode == GameMode.line)
            {
                foreach (SquarePos rayLine in rayLines)
                {
                    int count = 0;
                    foreach (SquarePos rayDirection in new SquarePos[] { rayLine, -rayLine })
                    {
                        SquarePos currentPosition = move;
                    }
                }
            }
            else
            {
                Debug.Log("In development, most lines game mode is not supported yet!");
            }
        }

        // Field types
        private enum Field
        {
            x,
            o,
            empty
        }

        // Game states
        public enum GameState
        {
            xWon,
            yWon,
            draw,
            onGoing
        }

        // Player to move
        public enum SideToMove
        {
            x,
            o
        }

        // Game mode
        public enum GameMode
        {
            line,
            mostLines
        }
    }
}
