using TMPro;
using UnityEngine;

namespace TokioSchool.Pong.UI
{
    public class UIWinnerController : MonoBehaviour
    {
        public TextMeshProUGUI winnerText;

        public string WinnerText { get => winnerText.text; set => winnerText.text = value; }
    }
}
