using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelGlobals;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameRunning, isGamePaused, isGameEnding;

    public float matchTime = 300f;
    public float elapsedMatchTime = 0f;
    public int score;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isGameEnding = false;
        isGameRunning = false;
        isGamePaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))//Esc button
            TogglePause();
        //elapsedMatchTime += Time.deltaTime;
        //if (elapsedMatchTime >= matchTime)
        //    StartCoroutine(EndGame(1f));
    }

    [Button]
    public void StartGame()
    {
        Time.timeScale = 1f;
        isGameRunning = true;
        isGameEnding = false;
        Moon.isActive = true;
        EnemiesSpawnManager.Instance.StartSpawn();
        UIManager.instance.ToggleGameUI(true);
    }

    [Button]
    public void EndGame()
    {
        StartCoroutine(EndGame(2));
    }

    public IEnumerator EndGame(float transitionDuration)
    {
        yield return new WaitForSecondsRealtime(2.5f);
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
        UIManager.instance.ToggleGameUI(false);
        UIManager.instance.SetFaderOpacity(1f);
        Time.timeScale = 1f;
        UIManager.instance.ToggleGameOverPanel(true);
    }

    public void TogglePause()
    {
        if (isGameEnding) return;
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        UIManager.instance.TogglePauseMenu(isGamePaused);
    }

    public void AddScore(int score)
    {
        this.score += score;
        UIManager.instance.SetScoreValue(this.score);
    }

    public void Restart()
    {
        SceneManager.instance.RestartScene();
    }

    public void ExitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}
