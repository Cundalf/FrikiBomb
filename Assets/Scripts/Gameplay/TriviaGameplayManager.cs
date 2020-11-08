using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaGameplayManager : MonoBehaviour
{
    public List<Image> Checks;
    public List<Question> Questions;
    public Sprite UncheckIcon;
    public Sprite OkIcon;
    public Text txtQuestion;
    public Text txtAnswer1;
    public Text txtAnswer2;
    public GameObject TriviaUI;

    public enum TriviaGameplayState { WAIT, QUESTION, END }

    public enum CorrectAnswer { LEFT = 0, RIGHT = 1 }

    public float TimeMinToComplete;
    private float timeToComplete;
    private float timeToCompleteCounter;

    private int randomQuestion;
    private int cantQuestionsAnswered;
    private List<int> questionsAnswered;
    private CorrectAnswer correctAnswer;
    private TriviaGameplayState actualState;

    private UiManager uiManager;
    private InputGameplayManager inputGameplayManager;

    void Start()
    {
        timeToComplete = TimeMinToComplete;
        actualState = TriviaGameplayState.WAIT;
        uiManager = FindObjectOfType<UiManager>().GetComponent<UiManager>();
        inputGameplayManager = FindObjectOfType<InputGameplayManager>().GetComponent<InputGameplayManager>();
    }

    private void Update()
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;

        if (actualState != TriviaGameplayState.QUESTION) return;
        timeToCompleteCounter += Time.deltaTime;

        if (timeToCompleteCounter >= timeToComplete)
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ERROR);
            FailAnswer();
            timeToCompleteCounter = 0;
        }
    }

    private void LateUpdate()
    {
        if (GameManager.SharedInstance.ActualGameState != GameManager.GameState.IN_GAME) return;
        if (actualState != TriviaGameplayState.QUESTION) return;
        uiManager.ChangeColorTimer(timeToCompleteCounter, timeToComplete);
    }

    public void StartGame()
    {
        for (int i = 0; i < Checks.Count; i++)
        {
            Checks[i].sprite = UncheckIcon;
        }

        questionsAnswered = new List<int>();
        cantQuestionsAnswered = 0;
        StartCoroutine(StartingGame());
    }

    IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(1f);
        TriviaUI.SetActive(true);
        NewQuestion();
        actualState = TriviaGameplayState.QUESTION;
        yield return null;
    }

    private void NewQuestion()
    {
        do
        {
            randomQuestion = Random.Range(0, Questions.Count);
        }
        while (questionsAnswered.Contains(randomQuestion));
        
        txtQuestion.text = Questions[randomQuestion].QuestionString;
        int randomOrder = Random.Range(0, 2);
        correctAnswer = (CorrectAnswer)randomOrder;

        if(correctAnswer == CorrectAnswer.LEFT)
        {
            txtAnswer1.text = Questions[randomQuestion].CorrectAnswer;
            txtAnswer2.text = Questions[randomQuestion].FakeAnswer;
        }
        else
        {
            txtAnswer2.text = Questions[randomQuestion].CorrectAnswer;
            txtAnswer1.text = Questions[randomQuestion].FakeAnswer;
        }
    }

    public void ValidateAnswer(CorrectAnswer opt)
    {
        if (actualState != TriviaGameplayState.QUESTION) return;

        timeToCompleteCounter = 0;

        if (opt == correctAnswer)
        {
            cantQuestionsAnswered++;
            questionsAnswered.Add(randomQuestion);
            Checks[cantQuestionsAnswered - 1].sprite = OkIcon;
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.CORRECT);

            if(Checks.Count == cantQuestionsAnswered)
            {
                actualState = TriviaGameplayState.END;
                uiManager.WinGame();
                GameManager.SharedInstance.ChangeGameManager(GameManager.GameState.GAME_OVER);
            }
            else
            {
                NewQuestion();
            }
        }
        else
        {
            FailAnswer();
        }
    }

    private void FailAnswer()
    {
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ERROR);
        actualState = TriviaGameplayState.WAIT;
        TriviaUI.SetActive(false);

        inputGameplayManager.StartGame();
    }
}
