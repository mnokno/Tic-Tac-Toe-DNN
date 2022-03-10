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
        private TrainingCenter trainingCenter = new TrainingCenter();

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
            else if (parts[0].ToLower() == "train" || parts[0].ToLower() == "t")
            {
                if (parts.Length == 3)
                {
                    try
                    {
                        trainingCenter.taskProgress.Reset();
                        Task.Run(() => trainingCenter.RunTrainingSession(int.Parse(parts[1]), int.Parse(parts[2])));
                        StartCoroutine(UpdateProgress(trainingCenter));
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                        CLI_UI.Log("Invalid Command", Color.red);
                        CLI_UI.SetLocked(false);
                    }
                }
                else if (parts.Length == 4)
                {
                    try
                    {
                        trainingCenter.taskProgress.Reset();
                        Task.Run(() => trainingCenter.RunTrainingSession(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
                        StartCoroutine(UpdateProgress(trainingCenter));
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                        CLI_UI.Log("Invalid Command", Color.red);
                        CLI_UI.SetLocked(false);
                    }
                }
                else if (parts.Length == 5)
                {
                    try
                    {
                        trainingCenter.taskProgress.Reset();
                        Task.Run(() => trainingCenter.RunTrainingSession(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
                        StartCoroutine(UpdateProgress(trainingCenter, "t " + parts[1] + " " + parts[2] + " " + parts[3] + " " + (int.Parse(parts[4]) - 1).ToString()));
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                        CLI_UI.Log("Invalid Command", Color.red);
                        CLI_UI.SetLocked(false);
                    }
                }
                else
                {
                    CLI_UI.Log("Invalid Command", Color.red);
                    CLI_UI.SetLocked(false);
                }
                //TrainingCenter trainingCenter = new TrainingCenter();
                //Task.Run(() => trainingCenter.SelectBest(TrainingCenter.GetRandomCandidates(int.Parse(parts[1]), 10, new int[] { 9, 9, 9, 8, 7, 6, 5, 4, 3, 2 }, 1), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4])));
                //StartCoroutine(UpdateProgress(trainingCenter));
            }
            else if (parts[0].ToLower() == "save" || parts[0].ToLower() == "s")
            {
                if (parts.Length == 2)
                {
                    trainingCenter.bestCandidates.Peek()[0].AI.brain.Save(parts[1]);
                    CLI_UI.Log("Saved successfully", Color.green);
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

        /// <summary>
        /// Updates progress of a training session
        /// </summary>
        private IEnumerator UpdateProgress(TrainingCenter tc)
        {
            // Shows progress
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

            // Shows top three networks
            Candidate[] topCandidates = trainingCenter.bestCandidates.Peek();
            string[] texts = new string[] { "T1: ", "T2: ", "T3: " };
            string message = "";
            if (topCandidates.Length < 3)
            {
                for (int i = 0; i < topCandidates.Length; i++)
                {
                    message += texts[i] + topCandidates[i].score + "\n";
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    message += texts[i] + topCandidates[i].score + "\n";
                }
            }
            CLI_UI.Log(message.Trim(), new Color(0, 0.75f, 0.75f, 1));
            CLI_UI.SetLocked(false);
        }

        /// <summary>
        /// Updates progress of a training session
        /// </summary>
        private IEnumerator UpdateProgress(TrainingCenter tc, string command)
        {
            // Calculates iteration
            int iteration = int.Parse(command.Split(" ")[4]) + 1;

            // Shows progress
            CLI_UI.Log($"RI: {iteration} Training Progress: 0.0%", Color.green);
            while (!tc.taskProgress.startedTraining)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
            while (tc.taskProgress.totalGamesToSimulate > tc.taskProgress.simulatedGames)
            {
                CLI_UI.UpdateTextOnPreviousLog($"RI: {iteration} Training Progress: {((float)tc.taskProgress.simulatedGames * 100 / (float)tc.taskProgress.totalGamesToSimulate).ToString("0.0")}%");
                yield return new WaitForSecondsRealtime(0.1f);
            }
            CLI_UI.UpdateTextOnPreviousLog($"RI: {iteration} Training Progress: 100.0%");

            // Shows top three networks
            Candidate[] topCandidates = trainingCenter.bestCandidates.Peek();
            string[] texts = new string[] { "T1: ", "T2: ", "T3: " };
            string message = "";
            if (topCandidates.Length < 3)
            {
                for (int i = 0; i < topCandidates.Length; i++)
                {
                    message += texts[i] + topCandidates[i].score + "\n";
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    message += texts[i] + topCandidates[i].score + "\n";
                }
            }
            CLI_UI.Log(message.Trim(), new Color(0, 0.75f, 0.75f, 1));
            CLI_UI.SetLocked(false);

            // Executes the command
            if (iteration - 1 > 0)
            {
                ExecuteCommand(command);
            }
        }
    }
}
