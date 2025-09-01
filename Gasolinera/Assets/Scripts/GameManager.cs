using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton sencillo

    public int score = 0;
    public float gameTime = 60f; // duración de la ronda
    private float timer;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private bool gameRunning = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = gameTime;
        UpdateUI();
    }

    void Update()
    {
        if (!gameRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            gameRunning = false;
            EndGame();
        }
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Puntos: " + score;
        timerText.text = "Tiempo: " + Mathf.CeilToInt(timer);
    }

    void EndGame()
    {
        Debug.Log("Fin de la ronda. Puntos: " + score);
        // Aquí luego podemos poner menú de reinicio
    }
}
