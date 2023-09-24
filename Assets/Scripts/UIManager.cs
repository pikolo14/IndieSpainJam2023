using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject pauseMenu, gameUI, gameOverPanel, tutorialPanel;
    public CanvasGroup fader;
    public Image healthBar;

    public TextMeshProUGUI txtScore;

    public GameObject TitleCanvas;
    public GameObject FirstTitle, RealTitle;

    private void Awake()
    {
        instance = this;
    }

    public void TogglePauseMenu(bool on)
    {
        pauseMenu.SetActive(on);
        ToggleTutorial(false);
    }

    public void ToggleTutorial(bool on)
    {
        tutorialPanel.SetActive(on);
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

    public void ToggleTitle(bool on)
    {
        TitleCanvas.SetActive(on);
        if(on)
        {
            if(PlayerPrefs.HasKey("AlreadyPlayed"))
            {
                RealTitle.SetActive(true);
                FirstTitle.SetActive(false);
            }
            else
            {
                RealTitle.SetActive(false);
                FirstTitle.SetActive(true);
            }
        }
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
