using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    // No dependemos de campos especiales en tu PlayerController
    public PlayerController controller; // arrástralo en el inspector
    private bool hasInstantFill = false;

    void Reset()
    {
        if (!controller) controller = GetComponent<PlayerController>();
    }

    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                StopAllCoroutines();
                StartCoroutine(SpeedBoost(duration));
                break;

            case PowerUpType.InstantFill:
                hasInstantFill = true; // se consumirá al servir un coche
                break;

            case PowerUpType.FreezeCars:
                StartCoroutine(FreezeCars(duration));
                break;
        }
    }

    public bool TryConsumeInstantFill()
    {
        if (!hasInstantFill) return false;
        hasInstantFill = false;
        return true;
    }

    private System.Collections.IEnumerator SpeedBoost(float duration)
    {
        if (!controller) yield break;

        // Usamos el mismo nombre de variable que ya tenías: 'speed'
        float original = controller.speed;
        controller.speed = original * 2f;
        yield return new WaitForSeconds(duration);
        controller.speed = original;
    }

    private System.Collections.IEnumerator FreezeCars(float duration)
    {
        var cars = FindObjectsOfType<CarPatience>();
        foreach (var c in cars) c.Freeze(true);
        yield return new WaitForSeconds(duration);
        // Solo descongelamos los que sigan vivos
        cars = FindObjectsOfType<CarPatience>();
        foreach (var c in cars) c.Freeze(false);
    }
}