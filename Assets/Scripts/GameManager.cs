using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        MAIN_MENU, IN_GAME, PAUSE, GAME_OVER
    }
    
    private GameState actualGameState;
    public GameState ActualGameState { 
        get
        {
            return actualGameState;
        }
    }

    // Singleton
    private static GameManager sharedInstance = null;

    public static GameManager SharedInstance
    {
        get
        {
            return sharedInstance;
        }
    }
    
    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(this);
    }

    public void ChangeGameManager(GameState newGameState)
    {
        actualGameState = newGameState;

        switch(actualGameState)
        {
            case GameState.MAIN_MENU:
                Time.timeScale = 1;
                break;
        }
    }

    public void PauseGame()
    {
        actualGameState = GameState.PAUSE;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        actualGameState = GameState.IN_GAME;
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
