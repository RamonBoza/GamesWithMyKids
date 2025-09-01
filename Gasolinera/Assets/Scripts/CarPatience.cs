using UnityEngine;
using UnityEngine.UI;

public class CarPatience : MonoBehaviour
{
    public float patienceTime = 10f;
    private float currentPatience;

    private CarSpawner spawner;
    public Slider patienceBar;
    private bool served = false; // 👈 bandera para saber si ya fue atendido

	private bool frozen = false;
	public void Freeze(bool on) { frozen = on; }

    void Start()
    {
        currentPatience = patienceTime;

        // Busca el Slider hijo
        
        if (patienceBar != null)
        {
            patienceBar.maxValue = patienceTime;
            patienceBar.value = currentPatience;
        }
    }

    public void SetSpawner(CarSpawner sp)
    {
        spawner = sp;
    }

    void Update()
    {
        if (served) return; // 👈 si ya fue atendido, no seguimos descontando paciencia

		if (!frozen)
    	{
        	currentPatience -= Time.deltaTime;

        	if (patienceBar != null)
        	{
            	patienceBar.value = currentPatience;
        	}

        	if (currentPatience <= 0f)
        	{
            	if (spawner != null)
            	{
                	spawner.CarLeft();
            	}
            	Destroy(gameObject);
        	}
		}
    }

    // 🔵 Llamar a esto cuando el jugador complete la acción de repostar
    public void ServeCar()
    {
        if (served) return; // por seguridad
        served = true;

        if (spawner != null)
        {
            spawner.CarServed();
        }

        Destroy(gameObject);
    }
}