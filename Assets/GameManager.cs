using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Questions")]
    public Question[] questions;

    [Header("UI Elements")]
    public Text qestionText;
    public Button[] answerButtons = new Button[3];
    public Text[] answersText = new Text[3]; //3 answer options
    public Image resultImage;
    public Text resultText;

    [Header("Animations")]
    public Animator gamePanelAnimator;

    [Header("Graphic")]
    public Sprite rightAnswerSprite;
    public Sprite wrongAnswerSprite;

    private List<Question> possibleQuestions; //unappeared questions that may still fall, but will not be repeated
    private Question currentQuestion;

    public void OnClickPlay()
    {
        possibleQuestions = new List<Question>(questions);

        QuestionGenerate();

        StartCoroutine(StartGameAnimation());
    }

    public void OnClickAnswer(int buttomIndex)
    {
        StartCoroutine(QuestionAndButtonsCloseAnimation());

        if (answersText[buttomIndex].text == currentQuestion.answers[0])
        {
            StartCoroutine(TrueOrFalseAnswer(true));
        }
        else
        {
            StartCoroutine(TrueOrFalseAnswer(false));
        }
    }

    private void QuestionGenerate()
    {

        int randQuestionIndex = Random.Range(0, possibleQuestions.Count);
        currentQuestion = possibleQuestions[randQuestionIndex];
        possibleQuestions.RemoveAt(randQuestionIndex);

        qestionText.text = currentQuestion.question; //filling UI items 

        List<string> copyAnswers = new List<string>(currentQuestion.answers);
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            int randAnswerIndex = Random.Range(0, copyAnswers.Count);

            answersText[i].text = copyAnswers[randAnswerIndex]; //filling UI items 

            copyAnswers.RemoveAt(randAnswerIndex);
        }
    }

    private IEnumerator TrueOrFalseAnswer(bool answerResult)
    {
        if (answerResult)
        {
            resultImage.sprite = rightAnswerSprite;
            resultText.text = "ПРАВИЛЬНЫЙ ОТВЕТ";
        }
        else
        {
            resultImage.sprite = wrongAnswerSprite;
            resultText.text = "НЕПРАВИЛЬНЫЙ ОТВЕТ";
        }


        yield return StartCoroutine(ResultImageBlinkAnimation());


        if (answerResult && possibleQuestions.Count > 0)
        {
            QuestionGenerate();

            StartCoroutine(QuestionAndButtonsOpenAnimation());
        }
        else
        {
            if(possibleQuestions.Count <= 0)
            {
                print("вопросы закончились"); //Какие либо действия при случае законтичвшихся вопросов
                //resultImage.sprite = questionsAreOverSprite;
                //resultText.text = "ВОПРОСЫ ЗАКОНЧИЛИСЬ";
            }

            StartCoroutine(EndGameAnimation());
        }

        yield break;
    }





    private IEnumerator StartGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetOut");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(QuestionAndButtonsOpenAnimation());

        yield break;
    }

    private IEnumerator EndGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetIn");
        yield return new WaitForSeconds(1f);

        yield break;
    }

    private IEnumerator QuestionAndButtonsOpenAnimation()
    {
        if (!qestionText.gameObject.activeSelf)
            qestionText.gameObject.SetActive(true);

        qestionText.gameObject.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(1f);


        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (!answerButtons[i].gameObject.activeSelf)
                answerButtons[i].gameObject.SetActive(true);

            answerButtons[i].gameObject.GetComponent<Animator>().SetTrigger("Open");
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < answerButtons.Length; i++) //buttons can be pressed
            answerButtons[i].interactable = true;

        yield break;
    }

    private IEnumerator QuestionAndButtonsCloseAnimation()
    {
        qestionText.gameObject.GetComponent<Animator>().SetTrigger("Close");


        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.GetComponent<Animator>().SetTrigger("Close");
        }

        yield return new WaitForSeconds(.17f);

        yield break;
    }

    private IEnumerator ResultImageBlinkAnimation()
    {
        yield return new WaitForSeconds(.2f);

        if (!resultImage.gameObject.activeSelf)
            resultImage.gameObject.SetActive(true);

        resultImage.gameObject.GetComponent<Animator>().SetTrigger("Open");

        yield return new WaitForSeconds(1f);

        resultImage.gameObject.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        if (resultImage.gameObject.activeSelf)
            resultImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(.2f);

        yield break;
    }
}

[System.Serializable]
public class Question
{
    public string question;
    [Tooltip("1-ый вариант ответа должен быть верным!")]
    public string[] answers = new string[3]; //3 answer options
}