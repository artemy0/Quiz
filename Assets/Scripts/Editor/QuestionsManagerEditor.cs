using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionsManager))]
public class QuestionsManagerEditor : Editor
{
    private QuestionsManager _questionsManager;
    private Question _newQuestion;

    private void OnEnable()
    {
        _questionsManager = ((QuestionsManager)target);
        _newQuestion = new Question();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QuestionsControlButtons(); //кнопки

        _newQuestion.question = EditorGUILayout.TextField("Question:", _newQuestion.question); //поля для удобного ввода вопроса и ответов
        EditorGUILayout.Space();
        _newQuestion.answers[0] = EditorGUILayout.TextField("Right answer:", _newQuestion.answers[0]);
        _newQuestion.answers[1] = EditorGUILayout.TextField("Wrong answer:", _newQuestion.answers[1]);
        _newQuestion.answers[2] = EditorGUILayout.TextField("Wrong answer:", _newQuestion.answers[2]);
    }

    private void QuestionsControlButtons()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal(); //распологаем 2-е кнопки в одном ряду

        AddQuestionButton();
        RemoveLastQuestionButton();

        GUILayout.EndHorizontal();
    }

    private void AddQuestionButton()
    {
        if (GUILayout.Button("Add question"))
        {
            _questionsManager.Questions.Add(_newQuestion);

            _newQuestion = new Question();
        }
    }

    private void RemoveLastQuestionButton()
    {
        if (GUILayout.Button("Remove last question"))
        {
            int lastIndex = _questionsManager.Questions.Count - 1;

            _questionsManager.Questions.RemoveAt(lastIndex);
        }
    }


}
