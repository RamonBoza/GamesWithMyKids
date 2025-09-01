using UnityEngine;

public class GasPumpZone : MonoBehaviour
{
    [Header("Setup")]
    public GameObject car;           // Asigna el coche activo
    public CarSpawner spawner;       // Asigna si usas spawner
    public float fillTime = 3f;      // Segundos para llenar
    public ProgressBar progressBar;  // Arrastra UI_RefuelBar aquí
    public KeyCode action1Key = KeyCode.Space; // Tecla de acción
	public KeyCode action2Key = KeyCode.Return; // Tecla de acción

    float progress = 0f;
    bool player1Inside = false;
	bool player2Inside = false;
    bool filling = false;

    public void AssignCar(GameObject newCar, CarSpawner spawnerRef)
    {
        car = newCar;
        spawner = spawnerRef;
        progress = 0f;
        filling = false;
        if (progressBar) { progressBar.SetProgress01(0f); progressBar.Show(false); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  player1Inside = true;
		if (other.CompareTag("Player2")) player2Inside = true;

		if ( player1Inside || player2Inside ) {
			var pp = other.GetComponent<PlayerPowerUps>();
	    	bool instant = pp && pp.TryConsumeInstantFill();
			if (instant) 
			{
				CompletarRepostaje();
			}
		}

		
    }

    void OnTriggerExit(Collider other)
    {
	    // si usas barra de progreso, salta al final si instant == true
	    // si sirves con tecla, simplemente llama ServeCar() y listo
	    
        if (other.CompareTag("Player"))
        {
            player1Inside = false;
        }
		if(other.CompareTag("Player2"))
		{
			player2Inside = false;
		}

		if(!player1Inside && !player2Inside) {
			filling = false;
            if (progressBar) progressBar.Show(false);
		}
    }

	void CompletarRepostaje()
	{
		// Completar
        if (progressBar) { progressBar.SetProgress01(1f); progressBar.Show(false); }
		CarPatience patience = car.GetComponent<CarPatience>();
		if (patience != null)
		{
    	patience.ServeCar();
		}
				
        GameManager.Instance.AddScore(10); // cada coche atendido da 10 puntos
		progress = 0f;
        filling = false;
	}

    void Update()
    {
        if (car == null) return;

		if (player1Inside && Input.GetKey(action1Key))
        {
	        Debug.Log("Jugador 1 llenando deposito");
            if (!filling)
            {
				Debug.Log("Haciendo filling J1");
                filling = true;
                if (progressBar) progressBar.Show(true);
            }

            progress += Time.deltaTime;
            if (progressBar) progressBar.SetProgress01(progress / fillTime);

            if (progress >= fillTime)
            {
                CompletarRepostaje();
            }
        }

		if (player2Inside && Input.GetKey(action2Key))
        {
			Debug.Log("Jugador 2 llenando deposito");
            if (!filling)
            {
                filling = true;
                if (progressBar) progressBar.Show(true);
            }

            progress += Time.deltaTime;
            if (progressBar) progressBar.SetProgress01(progress / fillTime);

            if (progress >= fillTime)
            {
                CompletarRepostaje();
            }
        }
        
		if(!(player1Inside && Input.GetKey(action1Key)) && !(player2Inside && Input.GetKey(action2Key)))
        {
            // Si sueltas la tecla o sales de zona, ocultar barra
            if (filling)
            {
                filling = false;
                if (progressBar) progressBar.Show(false);
            }
        }
    }
}
