using UnityEngine;

namespace TokioSchool.Pong.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public int speed;

        protected Rigidbody2D rb;
        protected Vector2 direction = Vector2.zero;

        [HideInInspector]
        public Vector2 Direction { get => direction; }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            DirectionPlayer();
        }

        private void FixedUpdate()
        {
            VelocityPlayer();
        }

        public virtual void DirectionPlayer()
        {
            direction = new Vector2(0, Input.GetAxisRaw("Vertical"));
        }

        public virtual void VelocityPlayer()
        {
            rb.velocity = direction * speed;
        }

        public void DecreasePlayerScale()
        {
            transform.localScale *= 0.8f;
        }
        public void IncreasePlayerScale()
        {
            transform.localScale *= 1.2f;
        }
    }
}
