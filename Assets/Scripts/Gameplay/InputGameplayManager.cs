using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputGameplayManager : MonoBehaviour
{
    public Image imgMSG;
    public Image imgInput;
    public List<int> CantPatterns;
    public List<Image> GameplayImages;
    public Sprite LeftIcon;
    public Sprite UpIcon;
    public Sprite DownIcon;
    public Sprite RightIcon;

    public Sprite OkSprite;
    public Sprite FailSprite;
    public Sprite QuestionSprite;

    public float TimeMinToComplete;
    private float timeToComplete;
    private float timeToCompleteCounter;

    public enum directions { UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3 }
    public enum InputGameplayState { WAIT, MSG, GAME, END }

    private InputGameplayState actualState;
    private int patternActual;
    private List<directions> pattern;
    private List<directions> inputs;
    private UiManager uiManager;
    private TriviaGameplayManager triviaGameplayManager;

    void Start()
    {
        HideGammeplayImages();
        imgInput.gameObject.SetActive(false);
        imgMSG.gameObject.SetActive(false);
        uiManager = FindObjectOfType<UiManager>().GetComponent<UiManager>();
        triviaGameplayManager = FindObjectOfType<TriviaGameplayManager>().GetComponent<TriviaGameplayManager>();

        StartGame();
    }
    
    public void StartGame()
    {
        HideGammeplayImages();
        pattern = new List<directions>();
        inputs = new List<directions>();

        imgMSG.gameObject.SetActive(false);
        imgInput.gameObject.SetActive(false);
        timeToComplete = TimeMinToComplete;
        patternActual = 0;
        timeToCompleteCounter = 0;
        GeneratePattern();
        actualState = InputGameplayState.GAME;
    }

    private void HideGammeplayImages()
    {
        foreach (Image img in GameplayImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;
        if (actualState != InputGameplayState.GAME) return;
        timeToCompleteCounter += Time.deltaTime;

        if (timeToCompleteCounter >= timeToComplete)
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ERROR);
            patternActual = 0;
            timeToCompleteCounter = 0;
            PatternFail();
        }
    }

    private void LateUpdate()
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;
        if (actualState != InputGameplayState.GAME) return;
        uiManager.ChangeColorTimer(timeToCompleteCounter, timeToComplete);
    }

    public void InputArrow(directions dir)
    {
        if (actualState != InputGameplayState.GAME) return;

        imgInput.sprite = GetIcon(dir);
        imgInput.gameObject.SetActive(true);

        inputs.Add(dir);

        if (dir != pattern[inputs.Count - 1])
        {
            patternActual = 0;
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ERROR);
            PatternFail();
            return;
        }

        if (inputs.Count == pattern.Count)
        {
            patternActual++;

            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.CORRECT);

            if (patternActual >= CantPatterns.Count)
            {
                actualState = InputGameplayState.END;
                HideGammeplayImages();
                imgMSG.sprite = QuestionSprite;
                imgMSG.color = new Color(0, 155, 255);
                imgMSG.gameObject.SetActive(true);
                triviaGameplayManager.StartGame();
            }
            else
            {
                PatternOK();
            }
        }
    }

    private void PatternOK()
    {
        HideGammeplayImages();
        actualState = InputGameplayState.MSG;
        imgMSG.sprite = OkSprite;
        imgMSG.color = new Color(0, 200, 0);
        imgMSG.gameObject.SetActive(true);
        //timeToComplete = TimeMinToComplete + patternActual;
        StartCoroutine(EndMSG());
    }

    private void PatternFail()
    {
        HideGammeplayImages();
        actualState = InputGameplayState.MSG;
        imgMSG.sprite = FailSprite;
        imgMSG.color = new Color(200, 0, 0);
        imgMSG.gameObject.SetActive(true);
        StartCoroutine(EndMSG());
    }

    IEnumerator EndMSG()
    {
        yield return new WaitForSeconds(1f);
        imgInput.gameObject.SetActive(false);
        imgMSG.gameObject.SetActive(false);
        GeneratePattern();
        inputs = new List<directions>();
        timeToCompleteCounter = 0;
        actualState = InputGameplayState.GAME;
        yield return null;
    }

    private void GeneratePattern()
    {
        if (patternActual >= CantPatterns.Count) return;

        pattern = new List<directions>();

        int cantPattern = CantPatterns[patternActual];
        int direction;
        for (int i = 0; i < cantPattern; i++)
        {
            direction = Random.Range(0, 4);
            GameplayImages[i].sprite = GetIcon((directions)direction);
            GameplayImages[i].gameObject.SetActive(true);
            pattern.Add((directions)direction);
        }
    }

    private Sprite GetIcon(directions dir)
    {
        if (dir == directions.UP) return UpIcon;
        if (dir == directions.RIGHT) return RightIcon;
        if (dir == directions.DOWN) return DownIcon;
        if (dir == directions.LEFT) return LeftIcon;

        Debug.LogWarning("Se ingreso un numero de icono invalido");
        return UpIcon;
    }
}
