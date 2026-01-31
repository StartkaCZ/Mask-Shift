using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _gameOverText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetScore(GameManager.Instance != null ? GameManager.Instance.Score : 0);
    }


    public void SetScore(int score)
    {
        if (_scoreText != null)
            _scoreText.text = $"{score}";
    }

    public void ShowGameOver(bool show)
    {
        if (_gameOverText != null)
            _gameOverText.enabled = show;
    }
}
