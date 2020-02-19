using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startGameButton;
    public Text questionText;
    public Button[] answerButtons;
    public Text[] answersText = new Text[3]; //3 answer options

    public Image resultImage;
    public Text resultText;
    public Text correctAnswersNumberText;
    public Image questionsAreOverImage;

    public Button continueGameButton;
    public Slider timerSlider;

    [Header("Graphic")]
    public Sprite rightAnswerSprite;
    public Sprite wrongAnswerSprite;

    [Header("ExternalScript")]
    public AdsManager adsManager;
    public AnimationManager animationManager;
    public QuestionsManager questionsManager;

    private Question currentQuestion;
    private int correctAnswersNumber;
    private bool haveSecondChance;

    public void OnClickPlay()
    {
        //Settings
        startGameButton.interactable = false;

        haveSecondChance = true; //Only once in a game session can you take a second chance

        correctAnswersNumber = 0;
        correctAnswersNumberText.text = correctAnswersNumber.ToString();
        questionsManager.RefreshQuestions();

        //UI question
        DetermineQuestion();
        StartCoroutine(animationManager.StartGameAnimation());

        //Ads
        adsManager.ShowBannerAds();
    }

    public void OnClickAnswer(int buttomIndex)
    {
        //UI question
        StartCoroutine(animationManager.CloseAnimation(questionText.gameObject));
        StartCoroutine(animationManager.CloseButtonsAnimation(answerButtons));

        //Answer logic
        StartCoroutine(TrueOrFalseAnswer(answersText[buttomIndex].text == currentQuestion.answers[0]));
    }

    private IEnumerator TrueOrFalseAnswer(bool answerResult)
    {
        DetermineAnswerResult(answerResult);
        yield return StartCoroutine(animationManager.BlinkAnimation(resultImage.gameObject));

        if (questionsManager.PossibleQuestionsCount <= 0)
        {
            yield return StartCoroutine(animationManager.BlinkAnimation(questionsAreOverImage.gameObject));
        }
        else if (answerResult)
        {
            yield return StartCoroutine(ShowNextQuestion());

            yield break;
        }
        else if (haveSecondChance)
        {
            //
            haveSecondChance = false;

            yield return StartCoroutine(ShowSecondChance());

            if (!timerSlider.GetComponent<TimerSlider>().IsTimeOver)
            {
                adsManager.ShowRewardedAds();
                yield return new WaitForSeconds(.3f); //Waiting for the launch of advertising

                yield return StartCoroutine(ShowNextQuestion());

                yield break;
            }
            //
        }

        yield return StartCoroutine(animationManager.BlinkAnimation(correctAnswersNumberText.gameObject));

        yield return StartCoroutine(animationManager.EndGameAnimation());

        //Ads
        adsManager.HideBannerAd();

        startGameButton.interactable = true;

        yield break;
    }

    private void DetermineQuestion()
    {
        //Question generation
        currentQuestion = questionsManager.GenerateUniqueQuestion();

        //UI display
        questionText.text = currentQuestion.question;

        List<string> copyAnswers = new List<string>(currentQuestion.answers);
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            int randAnswerIndex = Random.Range(0, copyAnswers.Count);

            answersText[i].text = copyAnswers[randAnswerIndex]; //filling UI items 

            copyAnswers.RemoveAt(randAnswerIndex);
        }
    }

    private void DetermineAnswerResult(bool answerResult)
    {
        if (answerResult)
        {
            correctAnswersNumber++;
            correctAnswersNumberText.text = correctAnswersNumber.ToString();

            resultImage.sprite = rightAnswerSprite;
            resultText.text = "ПРАВИЛЬНЫЙ ОТВЕТ";
        }
        else
        {
            resultImage.sprite = wrongAnswerSprite;
            resultText.text = "НЕПРАВИЛЬНЫЙ ОТВЕТ";
        }
    }

    private IEnumerator ShowNextQuestion()
    {
        DetermineQuestion();

        yield return StartCoroutine(animationManager.OpenAnimation(questionText.gameObject));
        yield return StartCoroutine(animationManager.OpenButtonsAnimation(answerButtons));

        yield break;
    }

    private IEnumerator ShowSecondChance()
    {
        yield return StartCoroutine(animationManager.OpenAnimation(continueGameButton.gameObject)); //continueGameButton.gameObject.SetActive(true);
        timerSlider.gameObject.SetActive(true);

        yield return new WaitWhile(() => { return timerSlider.GetComponent<TimerSlider>().IsTimerRunning; });

        timerSlider.gameObject.SetActive(false);
        yield return StartCoroutine(animationManager.CloseAnimation(continueGameButton.gameObject));
        continueGameButton.gameObject.SetActive(false);
    }
}