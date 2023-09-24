using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject pauseMenu, gameUI, gameOverPanel;
    public CanvasGroup fader;
    public Image healthBar;

    public TextMeshProUGUI txtScore;

    private void Awake()
    {
        instance = this;
    }

    public void TogglePauseMenu(bool on)
    {
        pauseMenu.SetActive(on);
    }

    public void ToggleGameUI(bool on)
    {
        gameUI.SetActive(on);
        gameUI.GetComponent<Animator>().SetBool("Show", on);
    }

    public void SetFaderOpacity(float opacity01)
    {
        fader.alpha = opacity01;
    }

    public void UpdateHealthPercentage(float remainingHPPorcentage)
    {
        healthBar.fillAmount = remainingHPPorcentage;
    }

    internal void SetScoreValue(int score)
    {
        txtScore.text = score.ToString("D6");
    }

    internal void ToggleGameOverPanel(bool on)
    {
        gameOverPanel.SetActive(on);
    }
}
