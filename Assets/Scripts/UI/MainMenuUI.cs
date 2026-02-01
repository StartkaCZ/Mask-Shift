using NUnit.Framework.Interfaces;
using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Disable these while in Main Menu / Countdown")]
    [SerializeField] Behaviour[] _gameplayBehaviours;

    [Header("Panels")]
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _countdownPanel;
    [SerializeField] GameObject _hudPanel;

    [Header("Text")]
    [SerializeField] TMP_Text   _countdownText;
    [SerializeField] TMP_Text   _highScoreMenuText;



    void Start()
    {
        Application.targetFrameRate = 144;
        // Ensure clean state on scene load
        GameManager.Instance?.ResetRunState();

        SetGameplayEnabled(false);

        _mainMenuPanel.SetActive(true);
        _hudPanel.SetActive(false);
        _countdownPanel.SetActive(false);

        int highscore = GameManager.Instance != null ? GameManager.Instance.HighScore : 0;
        if (_highScoreMenuText != null) _highScoreMenuText.text = $"{highscore}";
    }

    private void SetGameplayEnabled(bool enabled)
    {
        foreach (var behaviour in _gameplayBehaviours)
        {
            if (behaviour != null)
                behaviour.enabled = enabled;
        }
    }


    // Hook this to BTN_Play.onClick
    public void OnPlayPressed()
    {
        StartCoroutine(CountdownAndStart());
    }


    private IEnumerator CountdownAndStart()
    {
        _mainMenuPanel.SetActive(false);
        _countdownPanel.SetActive(true);

        // Make sure time is running for countdown (in case you died previously)
        Time.timeScale = 1f;

        yield return ShowCount("3");
        yield return ShowCount("2");
        yield return ShowCount("1");

        if (_countdownText != null) 
            _countdownText.text = "GO!";

        yield return new WaitForSecondsRealtime(0.5f);

        _hudPanel.SetActive(true);
        _countdownPanel.SetActive(false);

        GameManager.Instance?.ResetRunState();

        SetGameplayEnabled(true);
    }


    private IEnumerator ShowCount(string text)
    {
        _countdownText.text = text;
        yield return new WaitForSecondsRealtime(1f);
    }
}
