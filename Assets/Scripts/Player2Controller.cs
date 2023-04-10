using System.Collections;
using TokioSchool.Pong.Ball;
using UnityEngine;

namespace TokioSchool.Pong.Players
{
    public class Player2Controller : Player
    {
        public bool isAI = false;
        public BallBehaviour ball;
        public bool impulseBall = false;

        public override void DirectionPlayer()
        {
            if (isAI)
            {
                direction = ball.transform.position;
                direction.x = 0;
            }
            else
            {
                direction = new Vector2(0, Input.GetAxisRaw("Vertical2"));
            }
        }

        public override void VelocityPlayer()
        {
            if (isAI)
            {
                direction = ball.transform.position - transform.position;
                direction.x = 0;

                if (Mathf.Abs(direction.y) > 0.1f)
                {
                    rb.velocity = direction * speed;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                rb.velocity = direction * speed;
            }
        }

        public void StartRoundBehaviour()
        {
            if (isAI)
            {
                StartCoroutine(ImpulseBall());
            }
        }

        private IEnumerator ImpulseBall()
        {
            yield return new WaitForSeconds(3);
            impulseBall = true;
            yield return new WaitForSeconds(1);
            impulseBall = false;
        }
    }
}
