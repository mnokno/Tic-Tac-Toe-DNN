using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLI
{
    [RequireComponent(typeof(CLI_CommandManager))]
    public class CLI_UI : MonoBehaviour
    {
        public GameObject item;
        public Transform content;
        public TMPro.TMP_InputField inputField;
        public ScrollRect scrollRect;
        public CLI_CommandManager CLI_CommandManager;

        private bool locked = false;
        private Stack<GameObject> gameObjects = new Stack<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!locked && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab)))
            {
                if (inputField.text != "")
                {
                    // Logs the command
                    Log(inputField.text);

                    // Executes the command
                    CLI_CommandManager.ExecuteCommand(inputField.text);

                    // Clears 
                    inputField.text = "";
                    inputField.ActivateInputField();

                    // Scrolls to the bottom of the scroll rec
                    scrollRect.verticalNormalizedPosition = 0;
                    Canvas.ForceUpdateCanvases();
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                EraseLastLog();
            }
        }

        /// <summary>
        /// Logs a message to the CLI console
        /// </summary>
        public void Log(string log, Color color)
        {
            // Logs the command to the CLI console
            GameObject go = Instantiate(item);
            gameObjects.Push(go);
            go.transform.SetParent(content);
            go.transform.localScale = new Vector3(1, 1, 1);
            CLI_ScrolleViewItem scrolleViewItem = go.GetComponent<CLI_ScrolleViewItem>();
            scrolleViewItem.SetText(log);
            scrolleViewItem.SetColor(color);

            // Scrolls to the bottom of the scroll rec
            scrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
        }

        /// <summary>
        /// Logs a message to the CLI console
        /// </summary>
        public void Log(string log)
        {
            Log(log, Color.white);
        }

        /// <summary>
        /// Removes the last logged message
        /// </summary>
        public void EraseLastLog()
        {
            if (gameObjects.Count != 0)
            {
                Destroy(gameObjects.Pop().gameObject);
                scrollRect.verticalNormalizedPosition = 0;
                Canvas.ForceUpdateCanvases();
            }
        }

        /// <summary>
        /// Updates text on the most recent log
        /// </summary>
        public void UpdateTextOnPreviousLog(string text)
        {
            gameObjects.Peek().GetComponent<CLI_ScrolleViewItem>().SetText(text);
            Canvas.ForceUpdateCanvases();
        }

        /// <summary>
        /// Used to enable and disable the CLI's input field
        /// </summary>
        public void SetLocked(bool locked)
        {
            this.locked = locked;
        }
    }
}
