using UnityEngine;

public class CashRegisterZone : MonoBehaviour
{
    bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }

    void Update()
    {
		
        if (playerInside && ( Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))) // Player2 por ejemplo
        {
			Debug.Log("Jugador ha pagado");
            CarTask car = FindObjectOfType<CarTask>();
            if (car != null && car.GetCurrentTask() == TaskType.Pay)
            {
                if (car.CompleteCurrentTask())
                {
                    Destroy(car.gameObject);
                    FindObjectOfType<CarSpawner>().CarLeft();
                    GameManager.Instance.AddScore(10); // cada coche atendido da 10 puntos
                    Debug.Log("💵 Pago realizado, coche completado!");
                }
                else
                {
                    Debug.Log("Pago hecho, pero quedan tareas.");
                }
            }
        }
    }
}