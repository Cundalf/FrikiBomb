using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image imgTimer;
    public Text txtTime;
    public float levelTime;
    public GameObject frTimer;
    public GameObject frGameOver;
    public GameObject frTrivia;
    public GameObject frWin;
    public GameObject frPause;
    public Animator AnimatorCamera;
    public Animator AnimatorMegumin;

    private float currentTime;
    private float counter;

    void Start()
    {
        GameManager.SharedInstance.ChangeGameManager(GameManager.GameState.IN_GAME);
        frWin.SetActive(false);
        currentTime = levelTime;
    }

    void Update()
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;

        counter += Time.deltaTime;

        if(counter >= 1)
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.BOMB_SOUND);
            counter = 0;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            GameManager.SharedInstance.ChangeGameManager(GameManager.GameState.GAME_OVER);
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.EXPLOSION);
            GameOver();
            return;
        }

        txtTime.text = GetStringTime();
    }

    private string GetStringTime()
    {
        string minutes = Mathf.Floor(currentTime / 60).ToString("00");
        string seconds = (currentTime % 60).ToString("00");

        return minutes + ":" + seconds;
    }

    public void ChangeColorTimer(float actualTime, float totalTime)
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;

        float midTotalTime = totalTime / 2;
        float factor = ((actualTime * 100) / midTotalTime) / 100;
        if (actualTime <= midTotalTime)
        {
            imgTimer.color = new Color(factor, 1, 0);
        }
        else
        {
            imgTimer.color = new Color(1, 2 - factor, 0);
        }
    }

    public void WinGame()
    {
        frTrivia.SetActive(false);
        frTimer.SetActive(false);
        frWin.SetActive(true);

        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.WIN);
        AnimatorCamera.SetTrigger("EndGame");
        AnimatorMegumin.SetTrigger("EndGame");
    }

    public void GameOver()
    {
        AudioManager.SharedInstance.Stop();
        frTrivia.SetActive(false);
        frTimer.SetActive(false);
        frGameOver.SetActive(true);
    }

    public void GoToMainMenu()
    {
        GameManager.SharedInstance.ChangeGameManager(GameManager.GameState.MAIN_MENU);
        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeGame()
    {
        GameManager.SharedInstance.ResumeGame();
        frPause.SetActive(false);
    }

    public void ShowPauseUI()
    {
        frPause.SetActive(true);
    }
}
