using UnityEngine;

public enum PowerUpType { SpeedBoost, InstantFill, FreezeCars }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f;
    public float lifeTime = 25f; // si nadie lo recoge, desaparece

    private float t;
    private PowerUpSpawner spawner;

    public void SetSpawner(PowerUpSpawner s) { spawner = s; }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= lifeTime)
        {
            if (spawner) spawner.NotifyPowerUpGone(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var pp = other.GetComponent<PlayerPowerUps>();
        if (pp != null)
        {
            pp.ActivatePowerUp(type, duration);
            if (spawner) spawner.NotifyPowerUpGone(this);
            Destroy(gameObject);
        }
    }
}
