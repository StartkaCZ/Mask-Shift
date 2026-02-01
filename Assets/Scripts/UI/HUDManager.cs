using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _highscoreText;
    [SerializeField] TMP_Text _gameOverText;
    [SerializeField] TMP_Text _restartInstructionsText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetScore(GameManager.Instance != null ? GameManager.Instance.Score : 0);
        ShowGameOver(false);
    }


    public void SetScore(int score)
    {
        _scoreText.text = $"{score}";
    }

    public void SetHighScore(int highScore)
    {
        _highscoreText.text = $"{highScore}";
    }

    public void ShowGameOver(bool show)
    {
        if (_gameOverText != null)
            _gameOverText.enabled = show;
        if (_restartInstructionsText != null)
            _restartInstructionsText.enabled = show;
    }
}
