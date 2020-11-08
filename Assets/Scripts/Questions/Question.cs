using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Question")]
public class Question : ScriptableObject
{
    public string QuestionString;
    public string CorrectAnswer;
    public string FakeAnswer;
    public string Game;
}
