using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            CalculateSquaresToEdge();
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
            // Calculates the table
            bool IsValid(SquarePos squarePos)
            {
                if (squarePos.x < 0 || squarePos.y < 0 || squarePos.x > dimensions - 1 || squarePos.y > dimensions - 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
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
                        SquarePos currentPosition = new SquarePos(x, y) + rayDirections[i];
                        while (IsValid(currentPosition))
                        {
                            toEdge++;
                            currentPosition += rayDirections[i];
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

            Debug.Log(gameState);
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
            // Note, when this method is called we can assume that the game sate is onGoing
            if (gameMode == GameMode.line)
            {
                Field checkFor = sideToMove == SideToMove.o ? Field.x : Field.o;
                for (int i = 0; i < rayDirections.Length; i+=2)
                {
                    int count = 1;
                    for (int j = 0; j < 2; j++)
                    {
                        SquarePos currentPos = move;
                        int rayIndex = i + j;
                        for (int u = 0; u < squaresToEdge[rayIndex][move.x, move.y]; u++)
                        {
                            currentPos += rayDirections[rayIndex];
                            if (fields[currentPos.x, currentPos.y] != checkFor)
                            {
                                break;
                            }
                            else
                            {
                                count++;
                            }
                        }
                    }

                    if (count >= lineLenght)
                    {
                        gameState = sideToMove == SideToMove.o ? GameState.xWon : GameState.oWon;
                        return;
                    }
                }

                if (history.Count == dimensions * dimensions)
                {
                    gameState = GameState.draw;
                }
            }
            else
            {
                Debug.Log("In development, most lines game mode is not supported yet!");
            }
        }

        /// <summary>
        /// Returns true if given square is empty
        /// </summary>
        public bool IsEmpty(SquarePos squarePos)
        {
            return fields[squarePos.x, squarePos.y] == Field.empty;
        }

        /// <summary>
        /// Generates and returns an array of all legal moves
        /// </summary>
        public SquarePos[] GenerateLegalMoves()
        {
            List<SquarePos> legalMoves = new List<SquarePos>();
            for (int x = 0; x < dimensions; x++)
            {
                for (int y = 0; y < dimensions; y++)
                {
                    if (fields[x, y] == Field.empty)
                    {
                        legalMoves.Add(new SquarePos(x, y));
                    }
                }
            }

            return legalMoves.ToArray();
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
            oWon,
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
