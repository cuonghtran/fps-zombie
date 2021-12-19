using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeSurvive = 0;
    public int scoreAchieved = 0;

    private void OnEnable()
    {
        ZombieBehaviors.OnGrantScore += UpdateScore;
    }

    private void OnDisable()
    {

        ZombieBehaviors.OnGrantScore -= UpdateScore;
    }

    private void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timeSurvive += Time.deltaTime;
        GameboardUI.OnTimerChanged?.Invoke(timeSurvive);
    }

    void UpdateScore(int value)
    {
        scoreAchieved += value;
        GameboardUI.OnScoreChanged?.Invoke(scoreAchieved);
    }

    public void EndGame()
    {
        ResolveMenu.OnOpenResolveMenu?.Invoke(timeSurvive, scoreAchieved);
    }
}
