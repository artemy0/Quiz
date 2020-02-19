using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    [Header("Questions")]
    public List<Question> Questions;

    private List<Question> _possibleQuestions; //unappeared questions that may still fall, but will not be repeated

    public int PossibleQuestionsCount
    {
        get { return _possibleQuestions.Count; }
    }

    public void RefreshQuestions()
    {
        _possibleQuestions = new List<Question>(Questions);
    }

    public Question GenerateUniqueQuestion()
    {
        int randQuestionIndex = Random.Range(0, _possibleQuestions.Count);
        Question returnQuestion = _possibleQuestions[randQuestionIndex];
        _possibleQuestions.RemoveAt(randQuestionIndex);

        return returnQuestion;
    }
}
