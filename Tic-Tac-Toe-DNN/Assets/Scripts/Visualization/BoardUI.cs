using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visualization
{
    public class BoardUI : MonoBehaviour
    {
        public BoardSttings settings;
        public Color boardClolor;
        public Transform container;
        public Sprite squareSprite;

        private Camera cam;
        private float scale;

        private float longLength;
        private float shortLength;
        private float centerOffser;

        // Start is called before the first frame update
        private void Start()
        {
            // Finds camera
            cam = FindObjectOfType<Camera>();
            // Calculates scale
            scale = (settings.symbolSize * settings.dimensions) + (settings.lineWidth + settings.paddingLength * 2) * (settings.dimensions - 1);
            // Calculates square lengths and offsets, used to converts from world coordinates to a square position
            longLength = settings.symbolSize + settings.lineWidth + settings.paddingLength * 2;
            shortLength = settings.symbolSize + settings.lineWidth / 2f + settings.paddingLength;
            centerOffser = (settings.dimensions / 2f) * (settings.symbolSize) + ((settings.dimensions - 1) / 2f) * (settings.paddingLength * 2 + settings.lineWidth);
            // Generates the board
            DrawBoard();
        }

        // Generates a tic-tac-toe board based on the settings
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
                float xPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.paddingLength * (1 + 2 * i)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
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
                float yPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.paddingLength * (1 + 2 * i)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
                currentLine.transform.localPosition = new Vector3(0, yPos, 0);

                // Set correct scale
                currentLine.transform.localScale = new Vector3(scale, settings.lineWidth, 1);
            }
        }

        /// <summary>
        /// Converts given position to clicked square, returns -1 if the clicked position is not on this tic-tac-toe board
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector2 PositionToSquare(Vector2 position)
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
            // Returns square position
            return position.x < 0 | position.y < 0 ? new Vector2(-1, -1) : position;
        }

        void Update()
        {

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log(PositionToSquare(Input.mousePosition));
            }
        }

        [System.Serializable]
        public struct BoardSttings
        {
            public int dimensions;
            public float symbolSize;
            public float lineWidth;
            public float paddingLength;
        }
    }
}