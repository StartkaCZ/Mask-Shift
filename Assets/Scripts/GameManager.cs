using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    const string HIGHSCORE_KEY = "HighScore";
    const int DEFAULT_HIGHSCORE = 150;

    public int Score { get; private set; }
    public int HighScore { get; private set; }
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

        HighScore = PlayerPrefs.GetInt(HIGHSCORE_KEY, DEFAULT_HIGHSCORE);
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
        if (IsGameOver) return;

        Score += amount;
        HUDManager.Instance?.SetScore(Score);
    }


    public void ResetScore()
    {
        Score = 0;
        HUDManager.Instance?.SetScore(Score);
    }

    public void ResetRunState()
    {
        IsGameOver = false;
        Score = 0;
        Time.timeScale = 1f;

        HUDManager.Instance?.SetScore(Score);
        HUDManager.Instance?.SetHighScore(HighScore);
        HUDManager.Instance?.ShowGameOver(false);
    }


    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(HIGHSCORE_KEY, HighScore);
            PlayerPrefs.Save();
        }

        HUDManager.Instance?.SetHighScore(HighScore);
        HUDManager.Instance?.ShowGameOver(true);

        Time.timeScale = 0f;
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SceneManager.LoadScene("Game");
    }
}
