using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameRunning, isGamePaused, isGameEnding;

    public float matchTime = 300f;
    public float elapsedMatchTime = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isGameRunning = true;
        isGameEnding = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))//Esc button
            TogglePause();
        elapsedMatchTime += Time.deltaTime;
        if (elapsedMatchTime >= matchTime)
            StartCoroutine(EndGame(1f));
    }
    public IEnumerator EndGame(float transitionDuration)
    {
        isGameEnding = true;
        Time.timeScale = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            float alpha = Mathf.Lerp(0f, 1f, t);
            UIManager.instance.SetFaderOpacity(alpha);
            yield return null;
        }
        UIManager.instance.SetFaderOpacity(1f);
        Time.timeScale = 1f;
        SceneManager.instance.LoadScene("02_Credits");
    }

    public void TogglePause()
    {
        if (isGameEnding) return;
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        UIManager.instance.TogglePauseMenu(isGamePaused);
    }

}