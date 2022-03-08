using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLI
{
    public class CLI_ScrolleViewItem : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI lable;

        public void SetText(string text)
        {
            lable.text = text;
        }

        public void SetColor(Color color)
        {
            lable.color = color;
        }
    }
}