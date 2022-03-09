using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NN.Training;
using TicTacToe.UI.Visualization;
using System.Threading.Tasks;

namespace CLI
{
    public class CLI_CommandManager : MonoBehaviour
    {
        public BoardUI board;
        public CLI_UI CLI_UI;

        public void ExecuteCommand(string command)
        {
            // Locks the CLI's input field
            CLI_UI.SetLocked(true);

            // Replaces all ' ' (spaces) with '-' (minus)
            while (command.Contains(' '))
            {
                command = command.Replace(' ', '-');
            }

            // Splits the command into parts
            string[] parts = command.Split("-");

            // Executes the command
            if (parts[0].ToLower() == "load" || parts[0].ToLower() == "l")
            {
                if (parts.Length < 3) // Not a valid command
                {
                    CLI_UI.Log("Invalid Command", Color.red);
                    CLI_UI.SetLocked(false);
                }
                else if (parts[1].ToLower() == "b" || parts[1].ToLower() == "board")
                {
                    if (int.TryParse(parts[2], out int dimenstion))
                    {
                        CLI_UI.Log($"Loading board: {dimenstion}x{dimenstion}", Color.green);

                        // Destroys old chess board
                        foreach (Transform child in board.container)
                        {
                            Destroy(child.gameObject);
                        }

                        // Creates new chess board
                        board.settings.dimensions = dimenstion;
                        board.Start();

                        // Unlocks the CLI's input field
                        CLI_UI.SetLocked(false);
                    }
                    else
                    {
                        CLI_UI.Log("Invalid Command", Color.red);
                        CLI_UI.SetLocked(false);
                    }
                }
                else
                {
                    CLI_UI.Log("Invalid Command", Color.red);
                    CLI_UI.SetLocked(false);
                }
            }
            else if (parts[0].ToLower() == "test" || parts[0].ToLower() == "t")
            {
                TrainingCenter trainingCenter = new TrainingCenter();
                Task.Run(() => trainingCenter.SelectBest(TrainingCenter.GetRandomCandidates(int.Parse(parts[1]), 10, new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 }, 1), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4])));
                StartCoroutine(UpdateProgress(trainingCenter));
            }
            else
            {
                CLI_UI.Log("Invalid Command", Color.red);
                CLI_UI.SetLocked(false);
            }
        }

        /// <summary>
        /// Updates progress of a training session
        /// </summary>
        private IEnumerator UpdateProgress(TrainingCenter tc)
        {
            CLI_UI.Log($"Training Progress: 0.0%", Color.green);
            while (!tc.taskProgress.startedTraining)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
            while (tc.taskProgress.totalGamesToSimulate > tc.taskProgress.simulatedGames)
            {
                CLI_UI.UpdateTextOnPreviousLog($"Training Progress: {((float)tc.taskProgress.simulatedGames * 100 / (float)tc.taskProgress.totalGamesToSimulate).ToString("0.0")}%");
                yield return new WaitForSecondsRealtime(0.1f);
            }
            CLI_UI.UpdateTextOnPreviousLog($"Training Progress: 100.0%");
            CLI_UI.SetLocked(false);
        }
    }
}
