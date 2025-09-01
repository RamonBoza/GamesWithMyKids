using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;   // cada cuántos segundos puede aparecer un coche
    private float timer;
    private bool hasCar = false;
	public GasPumpZone pumpZone;

    private GameObject currentCar;

    void Update()
    {
        if (!hasCar)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnCar();
                timer = 0f;
            }
        }
    }

    private void SpawnCar()
    {
        currentCar = Instantiate(carPrefab, spawnPoint.position, Quaternion.identity);
        hasCar = true;

        // si el coche tiene paciencia, le asignamos este spawner
        CarPatience patience = currentCar.GetComponent<CarPatience>();
        if (patience != null)
        {
            patience.SetSpawner(this);
			pumpZone.AssignCar(currentCar, this);
        }
    }

    // 🔵 Llamado cuando el coche se fue enfadado (paciencia = 0)
    public void CarLeft()
    {
        Debug.Log("El coche se fue sin repostar 😡");
        hasCar = false;
        currentCar = null;

        // Aquí puedes restar puntos al jugador si quieres
        GameManager.Instance.AddScore(-1);
    }

    // 🟢 Llamado cuando el coche terminó de repostar
    public void CarServed()
    {
        Debug.Log("Coche atendido ✅");
        hasCar = false;
        currentCar = null;

        // Aquí puedes sumar puntos
        GameManager.Instance.AddScore(5);
    }
}
