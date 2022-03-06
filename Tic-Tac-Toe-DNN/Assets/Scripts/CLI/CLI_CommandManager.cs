using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.UI.Visualization;

namespace CLI
{
    public class CLI_CommandManager : MonoBehaviour
    {
        public BoardUI board;
        public CLI_UI CLI_UI;

        public void ExecuteCommand(string command)
        {
            // Replaces all ' ' (spaces) with '-' (minus)
            while (command.Contains(' '))
            {
                command = command.Replace('\n', '-');
            }

            // Splits the command into parts
            string[] parts = command.Split("-");

            // Executes the command
            if (parts[0].ToLower() == "load" || parts[0].ToLower() == "l")
            {
                if (parts.Length < 3) // Not a valid command
                {
                    CLI_UI.Log("Invalid Command", Color.red);
                }
                else if (parts[1].ToLower() == "b" || parts[1].ToLower() == "board")
                {
                    if (int.TryParse(parts[2], out int dimenstion))
                    {
                        CLI_UI.Log($"Loading board: {dimenstion}x{dimenstion}");

                        // Destroys old chess board
                        foreach (Transform child in board.container)
                        {
                            Destroy(child.gameObject);
                        }

                        // Creates new chess board
                        board.settings.dimensions = dimenstion;
                        board.Start();
                    }
                    else
                    {
                        CLI_UI.Log("Invalid Command", Color.red);
                    }
                }
                else
                {
                    CLI_UI.Log("Invalid Command", Color.red);
                }
            }
            else
            {
                CLI_UI.Log("Invalid Command", Color.red);
            }
        }
    }
}
