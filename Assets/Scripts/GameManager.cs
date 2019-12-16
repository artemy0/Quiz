using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Questions")]
    public Question[] questions;

    [Header("UI Elements")]
    public Button startGameButton;
    public Text qestionText;
    public Button[] answerButtons = new Button[3];
    public Text[] answersText = new Text[3]; //3 answer options
    public Image resultImage;
    public Text resultText;
    public Text correctAnswersNumberText;

    [Header("Graphic")]
    public Sprite rightAnswerSprite;
    public Sprite wrongAnswerSprite;

    [Header("ExternalScript")]
    public AdsManager adsManager;
    public AnimationManager animationManager;

    private List<Question> possibleQuestions; //unappeared questions that may still fall, but will not be repeated
    private Question currentQuestion;
    private int correctAnswersNumber;

    public void OnClickPlay()
    {
        possibleQuestions = new List<Question>(questions);
        correctAnswersNumber = 0;

        QuestionGenerate();

        StartCoroutine(animationManager.StartGameAnimation());

        //Ads part
        adsManager.ShowBannerAds();
    }

    public void OnClickAnswer(int buttomIndex)
    {
        StartCoroutine(animationManager.CloseAnimation());

        if (answersText[buttomIndex].text == currentQuestion.answers[0])
        {
            correctAnswersNumber++;
            correctAnswersNumberText.text = correctAnswersNumber.ToString();

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


        yield return StartCoroutine(animationManager.BlinkAnimation(resultImage.gameObject));


        if (answerResult && possibleQuestions.Count > 0)
        {
            QuestionGenerate();

            StartCoroutine(animationManager.OpenAnimation());
        }
        else
        {
            if(possibleQuestions.Count <= 0)
            {
                print("вопросы закончились"); //Какие либо действия при случае законтичвшихся вопросов
                //resultImage.sprite = questionsAreOverSprite;
                //resultText.text = "ВОПРОСЫ ЗАКОНЧИЛИСЬ";
            }

            yield return StartCoroutine(animationManager.BlinkAnimation(correctAnswersNumberText.gameObject));

            StartCoroutine(animationManager.EndGameAnimation());

            //Ads part
            adsManager.HideBannerAd();
            adsManager.ShowRewardedAds();
        }

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