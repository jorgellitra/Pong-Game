using System;
using UnityEngine;

namespace TokioSchool.Pong.PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        public PowerUpTypes type;
        public event Action onUse;

        public void Use()
        {
            if (onUse != null)
            {
                onUse();
                gameObject.GetComponentInParent<PowerUpSpawner>().currentPowerUps--;
                Destroy(gameObject);
            }
        }
    }
}
