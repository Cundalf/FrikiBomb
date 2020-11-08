using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject frTutorial;
    public GameObject frCredits;

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowTutorial()
    {
        frTutorial.SetActive(true);
    }

    public void HideTutorial()
    {
        frTutorial.SetActive(false);
    }

    public void ShowCredits()
    {
        frCredits.SetActive(true);
    }

    public void HideCredits()
    {
        frCredits.SetActive(false);
    }

    public void ExitGame()
    {
        GameManager.SharedInstance.ExitGame();
    }
}
