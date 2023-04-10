using System.Collections;
using TMPro;
using TokioSchool.Pong.GameController;
using TokioSchool.Pong.Players;
using UnityEngine;

namespace TokioSchool.Pong.UI
{
    public class UIScoreController : MonoBehaviour
    {
        [SerializeField] private int winConditionPoint = 10;
        [SerializeField] private TextMeshProUGUI player1Score;
        [SerializeField] private TextMeshProUGUI player2Score;
        [SerializeField] private PlayerController player1;
        [SerializeField] private Player2Controller player2;
        [SerializeField] private TextMeshProUGUI startTimerText;

        private int player1ScoreCounter = 0;
        private int player2ScoreCounter = 0;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            player1ScoreCounter = 0;
            player2ScoreCounter = 0;
            UpdateScores();
        }

        public void AddPointToPlayer1()
        {
            player1ScoreCounter++;
            UpdateScores();
        }

        public void AddPointToPlayer2()
        {
            player2ScoreCounter++;
            UpdateScores();
        }

        public void StartTimer(float timeToStart)
        {
            startTimerText.gameObject.SetActive(true);
            StartCoroutine(Timer(timeToStart));
        }

        private IEnumerator Timer(float timeToStart)
        {
            float startReset = timeToStart;
            while (startReset > 0f)
            {
                startReset -= Time.deltaTime;

                startTimerText.text = ((int)startReset).ToString();
                yield return null;
            }
            startTimerText.gameObject.SetActive(false);

            PongController.Instance.StartGame();
        }

        private void UpdateScores()
        {
            if (player1ScoreCounter == winConditionPoint)
            {
                PongController.Instance.FinishGame(player1);
            }
            else if (player2ScoreCounter == winConditionPoint)
            {
                PongController.Instance.FinishGame(player2);
            }
            else
            {
                player1Score.text = player1ScoreCounter.ToString();
                player2Score.text = player2ScoreCounter.ToString();
            }
        }
    }
}
