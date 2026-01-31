using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1f;
    }


    void Update()
    {
        if (Instance == null || !Instance.IsGameOver) return;

        if (Keyboard.current != null && 
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            Instance.Restart();
        }
    }


    public void AddScore(int amount)
    {
        Score += amount;
        HUDManager.Instance?.SetScore(Score);
    }


    public void ResetScore()
    {
        Score = 0;
        HUDManager.Instance?.SetScore(Score);
    }


    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        HUDManager.Instance?.ShowGameOver(true);

        // Freeze the run instantly (safe and simple)
        Time.timeScale = 0f;
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        ResetScore();

        // Reload your main scene (ensure the name matches)
        SceneManager.LoadScene("Game");
    }
}
