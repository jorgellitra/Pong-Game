using TokioSchool.Pong.GameController;
using UnityEngine;

namespace TokioSchool.Pong.PowerUps
{
    public class PowerUpSpawner : MonoBehaviour
    {
        public float spawnTimer = 8;
        public PowerUp[] powerUps;
        public int maxPowerUpSimultaneously = 2;
        public int currentPowerUps = 0;

        private float spawnTimerPowerUp = 0;

        void Start()
        {
            spawnTimerPowerUp = spawnTimer;
            currentPowerUps = 0;
        }

        void Update()
        {
            if (PongController.Instance.gameStarted)
            {
                spawnTimerPowerUp -= Time.deltaTime;

                if (spawnTimerPowerUp < 0 && currentPowerUps < maxPowerUpSimultaneously)
                {
                    spawnTimerPowerUp = spawnTimer;
                    SpawnPowerUp();
                }
            }
        }

        private void SpawnPowerUp()
        {
            int randomPowerUp = Random.Range(0, powerUps.Length - 1);
            int randomYPosition = Random.Range(-4, 4);

            GameObject powerUp = Instantiate(powerUps[randomPowerUp].gameObject, transform);
            powerUp.transform.localPosition = new Vector2(0, randomYPosition);
            PowerUp power = powerUp.GetComponent<PowerUp>();
            switch (power.type)
            {
                case PowerUpTypes.DecreaseOponentSize:
                    power.onUse += PongController.Instance.DecreaseOponentSize;
                    break;
                case PowerUpTypes.IncreasePlayerSize:
                    power.onUse += PongController.Instance.IncreasePlayerSize;
                    break;
                case PowerUpTypes.SwichtSpeedBall:
                    power.onUse += PongController.Instance.SwichtSpeedBall;
                    break;
                case PowerUpTypes.MoreBalls:
                    power.onUse += PongController.Instance.MoreBalls;
                    break;
                default:
                    break;
            }
            currentPowerUps++;
        }
    }
}
