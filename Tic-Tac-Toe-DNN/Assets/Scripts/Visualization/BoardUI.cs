using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Technical;

namespace TicTacToe.UI
{
    namespace Visualization
    {
        public class BoardUI : MonoBehaviour
        {
            public BoardSttings settings;
            public Color boardClolor;
            public Transform container;
            public Sprite squareSprite;
            public Sprite oSprite;
            public Sprite xSprite;

            private Camera cam;
            private float scale;

            private Vector2[,] centers;
            private SpriteRenderer[,] sprites;
            private float longLength;
            private float shortLength;
            private float centerOffser;
            private Board board;

            // Start is called before the first frame update
            public void Start()
            {
                // Finds camera
                cam = FindObjectOfType<Camera>();
                // Calculates scale
                scale = (settings.symbolSize * settings.dimensions) + (settings.lineWidth) * (settings.dimensions - 1);
                // Calculates square lengths and offsets, used to converts from world coordinates to a square position
                longLength = settings.symbolSize + settings.lineWidth;
                shortLength = settings.symbolSize + settings.lineWidth / 2f;
                centerOffser = (settings.dimensions / 2f) * (settings.symbolSize) + ((settings.dimensions - 1) / 2f) * (settings.lineWidth);
                // Creates an empty 2D array, and calculates centers
                centers = new Vector2[settings.dimensions, settings.dimensions];
                CalculateCenters();
                // Creates an empty 2D array, and creates empty sprites
                sprites = new SpriteRenderer[settings.dimensions, settings.dimensions];
                InitiateSprites();
                // Generates the board
                DrawBoard();
                // Create a new board representation
                board = new Board(settings.dimensions, settings.dimensions, Board.GameMode.line);

                // Test
                // NN.Training.TrainingCenter trainingCenter = new NN.Training.TrainingCenter();
                // trainingCenter.Test();
                Tests.Test.Run(1);
                //Tests.MergeSort.TimeSort(20, sortCandidates: true, logSorted: true);
            }

            // Update it called once per frame
            void Update()
            {

                if (Input.GetMouseButtonUp(0) && board.gameState == Board.GameState.onGoing)
                {
                    SquarePos clickedSquare = PositionToSquare(Input.mousePosition);
                    if (clickedSquare.x != -1 && clickedSquare.y != -1)
                    {
                        if (board.IsEmpty(clickedSquare))
                        {
                            UpdateSprite(board.sideToMove == Board.SideToMove.x ? Type.x : Type.o, clickedSquare);
                            board.MakeMove(clickedSquare, false);
                        }
                    }
                }
            }

            /// <summary>
            /// DrawBoard generates a tic-tac-toe board based on the settings
            /// </summary>
            private void DrawBoard()
            {
                // Generate vertical lines
                for (int i = 0; i < settings.dimensions - 1; i++)
                {
                    // Each line is generated as a rectangle
                    GameObject currentLine = new GameObject($"Vertical line {i + 1}");
                    currentLine.transform.parent = container;

                    // Adds the rectangle
                    SpriteRenderer spriteRenderer = currentLine.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = squareSprite;
                    spriteRenderer.color = boardClolor;

                    // Set correct location
                    float xPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
                    currentLine.transform.localPosition = new Vector3(xPos, 0, 0);

                    // Set correct scale
                    currentLine.transform.localScale = new Vector3(settings.lineWidth, scale, 1);
                }

                // Generate horizontal lines
                for (int i = 0; i < settings.dimensions - 1; i++)
                {
                    // Each line is generated as a rectangle
                    GameObject currentLine = new GameObject($"Horizontal line {i + 1}");
                    currentLine.transform.parent = container;

                    // Adds the rectangle
                    SpriteRenderer spriteRenderer = currentLine.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = squareSprite;
                    spriteRenderer.color = boardClolor;

                    // Set correct location
                    float yPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
                    currentLine.transform.localPosition = new Vector3(0, yPos, 0);

                    // Set correct scale
                    currentLine.transform.localScale = new Vector3(scale, settings.lineWidth, 1);
                }
            }

            /// <summary>
            /// Converts given position to clicked square, returns (-1, -1) if the clicked position is outside the bounds of this tic-tac-toe board
            /// </summary>
            private SquarePos PositionToSquare(Vector2 position)
            {
                int FloorDiv(float a)
                {
                    // If negative then its outsides the bounds of the board
                    if (a < 0)
                    {
                        return -1;
                    }

                    // Keep devision count
                    int counter = 0;

                    // Devision
                    while (true)
                    {
                        if (counter == 0 || counter == settings.dimensions - 1) // Special length (has edges only on one sides)
                        {
                            if (a > shortLength)
                            {
                                a -= shortLength;
                                counter++;
                            }
                            else
                            {
                                return counter;
                            }
                        }
                        else // Normal length (has edges on both sides)
                        {
                            if (a > longLength)
                            {
                                a -= longLength;
                                counter++;
                            }
                            else
                            {
                                return counter;
                            }
                        }
                    }
                }

                // Converts from screen to local coordinates
                position = cam.ScreenToWorldPoint(position) - new Vector3(-centerOffser, centerOffser, 0) - container.position;
                // Calculates square position
                position = new Vector2(FloorDiv(position.x), FloorDiv(position.y * -1));
                // Removes squares outside the rang of the board
                position = new Vector2(position.x < settings.dimensions ? position.x : -1, position.y < settings.dimensions ? position.y : -1);
                position = position.x < 0 | position.y < 0 ? new Vector2(-1, -1) : position;
                // Returns square position
                return (SquarePos)position;
            }

            /// <summary>
            /// Calculates centers of squares with current board settings
            /// </summary>
            private void CalculateCenters()
            {
                // Used to store primitive coordinates
                float[,] primitive = new float[2, settings.dimensions];

                // Calculates primitive coordinates
                Vector3 topRight = this.transform.position - new Vector3(centerOffser, centerOffser, 0);
                for (int i = 0; i < settings.dimensions; i++)
                {
                    primitive[0, i] = topRight.x + settings.symbolSize * (0.5f + i) + (settings.lineWidth) * i;
                    primitive[1, settings.dimensions - 1 - i] = topRight.y + settings.symbolSize * (0.5f + i) + (settings.lineWidth) * i;
                }

                // Calculates centers using primitive
                for (int x = 0; x < settings.dimensions; x++)
                {
                    for (int y = 0; y < settings.dimensions; y++)
                    {
                        centers[x, y] = new Vector2(primitive[0, x], primitive[1, y]);
                    }
                }
            }

            /// <summary>
            /// Creates an empty sprite for each square
            /// </summary>
            private void InitiateSprites()
            {
                for (int x = 0; x < settings.dimensions; x++)
                {
                    for (int y = 0; y < settings.dimensions; y++)
                    {
                        GameObject go = new GameObject($"x:{x}, y:{y}");
                        go.transform.SetParent(container);
                        go.transform.position = centers[x, y];
                        go.transform.localScale = new Vector3(settings.symbolSize / 2f, settings.symbolSize / 2f, 1);
                        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                        sr.color = new Color(1, 1, 1, 1);
                        sprites[x, y] = sr;
                    }
                }
            }

            /// <summary>
            /// Updates displayed sprites, with the given coordinates
            /// </summary>
            private void UpdateSprite(Type type, SquarePos move)
            {
                switch (type)
                {
                    case Type.x:
                        sprites[move.x, move.y].sprite = xSprite;
                        break;
                    case Type.o:
                        sprites[move.x, move.y].sprite = oSprite;
                        break;
                    default:
                        sprites[move.x, move.y].sprite = null;
                        break;
                }
            }

            // Groups board settings
            [System.Serializable]
            public struct BoardSttings
            {
                public int dimensions;
                public float symbolSize;
                public float lineWidth;
            }

            // Type of square occupation
            public enum Type
            {
                x,
                o,
                empty
            }
        }
    }
}