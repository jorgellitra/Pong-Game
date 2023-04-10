using System;
using TokioSchool.Pong.Ball;
using TokioSchool.Pong.Players;
using TokioSchool.Pong.PowerUps;
using TokioSchool.Pong.UI;
using UnityEngine;

namespace TokioSchool.Pong.GameController
{
    public class PongController : MonoBehaviour
    {
        [SerializeField] private BallBehaviour ball;
        [SerializeField] private PlayerController player1;
        [SerializeField] private Player2Controller player2;
        [SerializeField] private BoxCollider2D goalTriggerPlayer1;
        [SerializeField] private BoxCollider2D goalTriggerPlayer2;
        [SerializeField] private Vector3 initialLocalScalePlayer = new Vector3(0.3f, 1.3f, 1f);
        public float timeToStart = 5f;

        [HideInInspector]
        public bool gameStarted = false;

        [SerializeField] private UIController uIController;
        [SerializeField] private UIWinnerController uIWinnerController;
        [SerializeField] private UIScoreController uIScoreController;

        private static PongController instance;

        public static PongController Instance { get => instance; set => instance = value; }

        private void Awake()
        {
            instance = this;
        }
        public void StartGame()
        {
            gameStarted = true;
            ball.StartGameMovement();
        }

        public void RestartGame()
        {
            uIScoreController.StartTimer(timeToStart);
        }

        public void StartGame2Players()
        {
            uIScoreController.StartTimer(timeToStart);
            player2.isAI = false;
            player2.speed = player1.speed;
        }

        public void StartGameVSAIPlayers()
        {
            uIScoreController.StartTimer(timeToStart);
            player2.isAI = true;
        }

        public void StartNewRound(Transform winnerRoundPlayer)
        {
            ball.GoToPlayerPosition(winnerRoundPlayer);
            ball.CanStartRound(winnerRoundPlayer);
        }

        public void FinishGame(Player winnerGame)
        {
            gameStarted = false;
            uIController.SwitchScreens(uIWinnerController.GetComponent<UIPanel>());
            ball.transform.position = Vector3.zero;
            ball.canStartRound = false;
            uIScoreController.StartGame();
            player1.transform.localScale = initialLocalScalePlayer;
            player2.transform.localScale = initialLocalScalePlayer;

            if (winnerGame.GetComponent<Player>().GetType() == typeof(PlayerController))
            {
                uIWinnerController.WinnerText = "Player 1 has won the game! Congratulations.";
            }
            else
            {
                uIWinnerController.WinnerText = "Player 2 has won the game! Congratulations.";
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                ball.PlayAudioGoal();

                GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                foreach (GameObject ball in balls)
                {
                    if (ball.GetComponent<BallBehaviour>().Id != this.ball.Id)
                    {
                        Destroy(ball);
                    }
                }

                if (goalTriggerPlayer1.IsTouching(ball.GetComponent<BoxCollider2D>()))
                {
                    StartNewRound(player1.transform);
                    uIScoreController.AddPointToPlayer1();
                }
                else if (goalTriggerPlayer2.IsTouching(ball.GetComponent<BoxCollider2D>()))
                {
                    StartNewRound(player2.transform);
                    player2.StartRoundBehaviour();
                    uIScoreController.AddPointToPlayer2();
                }
            }
        }

        public void DecreaseOponentSize()
        {
            Player player = ball.lasPlayerHit;

            if (player.GetType() == typeof(PlayerController))
            {
                player2.DecreasePlayerScale();
            }
            else
            {
                player1.DecreasePlayerScale();
            }
        }

        public void IncreasePlayerSize()
        {
            ball.lasPlayerHit.IncreasePlayerScale();
        }

        public void SwichtSpeedBall()
        {
            ball.SwichtSpeedBall();
        }

        public void MoreBalls()
        {
            GameObject newBall = Instantiate(ball.gameObject, transform.position, Quaternion.identity);
            newBall.transform.position = ball.transform.position - ball.transform.up * 2;
            BallBehaviour newBallBehaviour = newBall.GetComponent<BallBehaviour>();
            newBallBehaviour.direction = ball.direction;
            newBallBehaviour.direction.y *= -1;
            newBallBehaviour.lasPlayerHit = ball.lasPlayerHit;
        }
    }
}
