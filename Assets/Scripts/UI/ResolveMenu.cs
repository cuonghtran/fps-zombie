using System;
using UnityEngine;
using TMPro;

public class ResolveMenu : MonoBehaviour
{
    [SerializeField] GameObject _resolvePanel;
    [SerializeField] TMP_Text _timeSurvive_Text;
    [SerializeField] TMP_Text _score_Text;
    public static bool gameIsPaused;
    public static Action<float, int> OnOpenResolveMenu;

    private void OnEnable()
    {
        OnOpenResolveMenu += ResolveSequence;
    }

    private void OnDisable()
    {
        OnOpenResolveMenu -= ResolveSequence;
    }

    void ResolveSequence(float timeSurvive, int scoreValue)
    {
        _timeSurvive_Text.text = Mathf.RoundToInt(timeSurvive).ToString() + "s";
        _score_Text.text = scoreValue.ToString();
        _resolvePanel.SetActive(true);
        FreeCursor();
        PauseGame(true);
    }

    public void PlayAgainButton_Click()
    {
        SceneController.Instance.FadeAndLoadScene(ConstantsList.Scenes["OpeningScene"]);
        PauseGame(false);
    }

    private void PauseGame(bool isPaused)
    {
        gameIsPaused = isPaused;
        if (gameIsPaused)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    void FreeCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
