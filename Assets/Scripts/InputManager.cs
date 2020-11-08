using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public UiManager uiManager;
    public InputGameplayManager inputGameplayManager;
    public TriviaGameplayManager triviaGameplayManager;

    private void Update()
    {
        if (GameManager.SharedInstance.ActualGameState == GameManager.GameState.IN_GAME)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputGameplayManager.InputArrow(InputGameplayManager.directions.LEFT);
                triviaGameplayManager.ValidateAnswer(TriviaGameplayManager.CorrectAnswer.LEFT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputGameplayManager.InputArrow(InputGameplayManager.directions.UP);
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputGameplayManager.InputArrow(InputGameplayManager.directions.RIGHT);
                triviaGameplayManager.ValidateAnswer(TriviaGameplayManager.CorrectAnswer.RIGHT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputGameplayManager.InputArrow(InputGameplayManager.directions.DOWN);
                return;
            }

        }

        // Menu (default: Escape)
        if (Input.GetButtonUp("Cancel"))
        {
            switch (GameManager.SharedInstance.ActualGameState)
            {
                case GameManager.GameState.IN_GAME:
                    uiManager.ShowPauseUI();
                    GameManager.SharedInstance.PauseGame();
                    break;
                case GameManager.GameState.PAUSE:
                    uiManager.ResumeGame();
                    GameManager.SharedInstance.ResumeGame();
                    break;
            }
            return;
        }
    }
}