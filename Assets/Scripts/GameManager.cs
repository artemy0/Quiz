using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button StartGameButton;
    [SerializeField] private TimerSlider TimerSlider;

    [SerializeField] private Text QuestionText;
    [SerializeField] private Button[] AnswerButtons;
    [SerializeField] private Text[] AnswersText; //3 answer options
    [SerializeField] private Image ResultImage;
    [SerializeField] private Text ResultText;
    [SerializeField] private Text CorrectAnswersNumberText;
    [SerializeField] private Image QuestionsAreOverImage;
    [SerializeField] private Button ContinueGameButton;

    [Header("ExternalScript")]
    [SerializeField] private AdsManager AdsManager;
    [SerializeField] private AnimationManager AnimationManager;
    [SerializeField] private QuestionsManager QuestionsManager;

    [SerializeField] private Sprite RightAnswerSprite;
    [SerializeField] private Sprite WrongAnswerSprite;

    private Question _currentQuestion;
    private int _correctAnswersNumber;
    private bool _haveSecondChance;

    public void OnClickPlay()
    {
        //Settings
        SetInitialSettings();

        //UI question
        DetermineQuestion();

        //Animation
        AnimationManager.CallStartAnimation(QuestionText, AnswerButtons);

        //Ads
        AdsManager.ShowBannerAds();
    }

    public void OnClickAnswer(int buttomIndex)
    {
        //Animation
        AnimationManager.CallCloseAnimation(QuestionText.gameObject);
        AnimationManager.CallCloseAnimation(AnswerButtons);

        //Answer logic
        StartCoroutine(TrueOrFalseAnswer(AnswersText[buttomIndex].text == _currentQuestion.answers[0]));
    }

    private void SetInitialSettings()
    {
        StartGameButton.interactable = false;
        QuestionsManager.RefreshQuestions();

        _haveSecondChance = true;
        _correctAnswersNumber = 0;
    }

    private void SetFinalSettings()
    {
        StartGameButton.interactable = true;
    }

    private IEnumerator TrueOrFalseAnswer(bool answerResult)
    {
        DetermineAnswerResult(answerResult);
        yield return AnimationManager.CallBlinkAnimation(ResultImage.gameObject);

        if (QuestionsManager.QuestionsAreOver)
        {
            yield return AnimationManager.CallBlinkAnimation(QuestionsAreOverImage.gameObject);
        }
        else if (answerResult)
        {
            yield return StartCoroutine(ShowNextQuestion());

            yield break; //if the answer is correct, we go to the next question
        }
        else if (_haveSecondChance)
        {
            _haveSecondChance = false;

            yield return AnimationManager.CallSecondChance(ContinueGameButton, TimerSlider);

            if (!TimerSlider.GetComponent<TimerSlider>().IsTimeOver)
            {
                AdsManager.ShowRewardedAds();
                yield return new WaitForSeconds(.3f); //Waiting for the launch of advertising

                yield return StartCoroutine(ShowNextQuestion());

                yield break; //if the answer is not correct, but the second chance has been used, we go to the next question
            }
        }

        //in the case if the questions ended or we answered incorrectly and we did not have a second chance or we have a second chance but we did not use it, then the test ends
        yield return AnimationManager.CallBlinkAnimation(CorrectAnswersNumberText.gameObject);

        yield return AnimationManager.CallEndAnimation();

        //Settings
        SetFinalSettings();

        //Ads
        AdsManager.HideBannerAd();

        yield break;
    }

    private IEnumerator ShowNextQuestion()
    {
        DetermineQuestion();

        //Animations
        yield return AnimationManager.CallOpenAnimation(QuestionText.gameObject);
        yield return AnimationManager.CallOpenAnimation(AnswerButtons);

        yield break;
    }

    private void DetermineQuestion()
    {
        //Question generation
        _currentQuestion = QuestionsManager.GenerateUniqueQuestion();

        //UI display
        QuestionText.text = _currentQuestion.question;

        List<string> copyAnswers = new List<string>(_currentQuestion.answers);
        for (int i = 0; i < _currentQuestion.answers.Length; i++)
        {
            int randAnswerIndex = Random.Range(0, copyAnswers.Count);

            AnswersText[i].text = copyAnswers[randAnswerIndex]; //filling UI items 

            copyAnswers.RemoveAt(randAnswerIndex);
        }
    }

    private void DetermineAnswerResult(bool answerResult)
    {
        if (answerResult)
        {
            _correctAnswersNumber++;

            ResultImage.sprite = RightAnswerSprite;
            ResultText.text = "ПРАВИЛЬНЫЙ ОТВЕТ";
        }
        else
        {
            ResultImage.sprite = WrongAnswerSprite;
            ResultText.text = "НЕПРАВИЛЬНЫЙ ОТВЕТ";
        }

        CorrectAnswersNumberText.text = _correctAnswersNumber.ToString();
    }
}