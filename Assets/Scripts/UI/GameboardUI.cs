using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class GameboardUI : MonoBehaviour
{
    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _scoreText;

    float _timer;
    float _score;

    public static Action<float> OnTimerChanged;
    public static Action<int> OnScoreChanged;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
        _score = 0;
    }

    private void OnEnable()
    {
        OnTimerChanged += UpdateTimer;
        OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        OnTimerChanged -= UpdateTimer;
        OnScoreChanged -= UpdateScore;
    }

    void UpdateTimer(float value)
    {
        _timerText.text = Mathf.RoundToInt(value).ToString() + "s";
    }

    void UpdateScore(int value)
    {
        _scoreText.text = value.ToString();
    }
}
