using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject pauseMenu;
    public CanvasGroup fader;

    private void Awake()
    {
        instance = this;
    }

    public void TogglePauseMenu(bool on)
    {
        pauseMenu.SetActive(on);
    }

    public void SetFaderOpacity(float opacity01)
    {
        fader.alpha = opacity01;
    }

    public void UpdateHealth(int remainingHP)
    {
        throw new NotImplementedException();
    }
}
