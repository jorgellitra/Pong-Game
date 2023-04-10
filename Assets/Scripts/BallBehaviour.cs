using TokioSchool.Pong.Players;
using TokioSchool.Pong.PowerUps;
using UnityEngine;

namespace TokioSchool.Pong.Ball
{
    public class BallBehaviour : MonoBehaviour
    {
        public float speed = 5f;
        public int upgradeVelocityInHitCounter = 0;
        public AudioSource audioHit;
        public AudioSource audioGoal;
        [HideInInspector]
        public Player lasPlayerHit;
        [HideInInspector]
        public Player powerUpSwichtSpeedBall;
        [HideInInspector]
        public Vector2 direction = Vector2.zero;
        [HideInInspector]
        public string Id;
        public bool canStartRound = false;

        private int hitCounterMultiplyer = 0;
        private Rigidbody2D rb;

        private float initialSpeed;
        private Transform playerRoundStarter;

        private void Start()
        {
            Id = System.Guid.NewGuid().ToString();
            rb = GetComponent<Rigidbody2D>();
            initialSpeed = speed;
        }

        private void Update()
        {
            if (canStartRound)
            {
                if (playerRoundStarter.GetComponent<Player>().GetType() == typeof(PlayerController))
                {
                    if (Input.GetButtonDown("Vertical"))
                    {
                        canStartRound = false;
                        direction = Vector2.right;
                    }
                }
                else if (playerRoundStarter.GetComponent<Player>().GetType() == typeof(Player2Controller))
                {
                    Player2Controller player2Controller = (Player2Controller)playerRoundStarter.GetComponent<Player>();
                    if (player2Controller.isAI)
                    {
                        if (player2Controller.impulseBall)
                        {
                            canStartRound = false;
                            direction = Vector2.left;
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("Vertical2"))
                        {
                            canStartRound = false;
                            direction = Vector2.left;
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = direction * speed;
        }

        public void StartGameMovement()
        {
            int leftOrRight = Random.Range(-1f, 1f) > 0 ? -1 : -1;

            direction = new Vector2(leftOrRight, Random.Range(-.8f, .8f));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enviroment"))
            {
                PlayAudioHit();
                direction.y *= -1;
            }
            else if (collision.collider.CompareTag("Player"))
            {
                lasPlayerHit = collision.gameObject.GetComponent<Player>();
                PlayAudioHit();
                AddHitCounterMultiplyer();
                direction.x *= -1;
                direction.y += Mathf.Clamp(lasPlayerHit.Direction.y, -.8f, .8f);

                if (powerUpSwichtSpeedBall != null)
                {
                    if (powerUpSwichtSpeedBall == lasPlayerHit)
                    {
                        speed *= 1.3f;
                    }
                    else
                    {
                        speed *= 0.7f;
                    }
                }
            }
            else if (collision.collider.CompareTag("Ball"))
            {
                PlayAudioHit();
                if ((collision.collider.GetComponent<BallBehaviour>().direction.x >= 0 && direction.x >= 0) ||
                    collision.collider.GetComponent<BallBehaviour>().direction.x < 0 && direction.x < 0)
                {
                    direction.y *= -1;
                }
                else
                {
                    direction.x *= -1;
                    direction.y *= -1;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PowerUp"))
            {
                collision.GetComponent<PowerUp>().Use();
            }
        }

        public void GoToPlayerPosition(Transform player)
        {
            direction = Vector2.zero;
            speed = initialSpeed;

            if (player.position.x > 0)
            {
                transform.position = player.position - player.transform.right;
            }
            else
            {
                transform.position = player.position + player.transform.right;
            }
        }

        public void CanStartRound(Transform winnerRoundPlayer)
        {
            canStartRound = true;
            playerRoundStarter = winnerRoundPlayer;
            lasPlayerHit = winnerRoundPlayer.GetComponent<Player>();
        }

        public void AddHitCounterMultiplyer()
        {
            hitCounterMultiplyer++;

            if (hitCounterMultiplyer == upgradeVelocityInHitCounter)
            {
                hitCounterMultiplyer = 0;
                speed *= 1.1f;
            }
        }

        public void SwichtSpeedBall()
        {
            powerUpSwichtSpeedBall = lasPlayerHit;
        }

        public void PlayAudioHit()
        {
            audioHit.Play();
        }

        public void PlayAudioGoal()
        {
            audioGoal.Play();
        }
    }
}
