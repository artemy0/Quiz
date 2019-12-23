using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    [Header("Questions")]
    public Question[] questions;

    private List<Question> possibleQuestions; //unappeared questions that may still fall, but will not be repeated

    public int possibleQuestionsCount
    {
        get { return possibleQuestions.Count; }
    }

    public void RefreshQuestions()
    {
        possibleQuestions = new List<Question>(questions);
    }

    public Question GenerateUniqueQuestion()
    {
        int randQuestionIndex = Random.Range(0, possibleQuestions.Count);
        Question returnQuestion = possibleQuestions[randQuestionIndex];
        possibleQuestions.RemoveAt(randQuestionIndex);

        return returnQuestion;
    }
}
