using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Text QuestionText;
    [SerializeField] private Button[] AnswerButtons;
    [SerializeField] private Text[] AnswersText = new Text[3]; //3 answer options
    [SerializeField] private Image ResultImage;
    [SerializeField] private Text ResultText;
    [SerializeField] private Text CorrectAnswersNumberText;
    [SerializeField] private Image QuestionsAreOverImage;
    [SerializeField] private TimerSlider TimerSlider;

    [Header("Graphic")]
    [SerializeField] private Sprite RightAnswerSprite;
    [SerializeField] private Sprite WrongAnswerSprite;

    [Header("ExternalScript")]
    [SerializeField] private AdsManager AdsManager;
    [SerializeField] private AnimationManager AnimationManager;
    [SerializeField] private QuestionsManager QuestionsManager;

    private Question _currentQuestion;
    private int _correctAnswersNumber;
    private bool _haveSecondChance;

    public void OnClickPlay()
    {
        //Settings
        StartGameButton.interactable = false;
        QuestionsManager.RefreshQuestions();

        _haveSecondChance = true;
        _correctAnswersNumber = 0;
        CorrectAnswersNumberText.text = _correctAnswersNumber.ToString();

        //UI question
        DetermineQuestion();
        AnimationManager.CallStartAnimation(QuestionText, AnswerButtons);

        //Ads
        AdsManager.ShowBannerAds();
    }

    public void OnClickAnswer(int buttomIndex)
    {
        //UI question
        AnimationManager.CallCloseAnimation(QuestionText.gameObject);
        AnimationManager.CallCloseAnimation(AnswerButtons);

        //Answer logic
        StartCoroutine(TrueOrFalseAnswer(AnswersText[buttomIndex].text == _currentQuestion.answers[0]));
    }

    private IEnumerator TrueOrFalseAnswer(bool answerResult)
    {
        DetermineAnswerResult(answerResult);
        yield return AnimationManager.CallBlinkAnimation(ResultImage.gameObject);

        if (QuestionsManager.PossibleQuestionsCount <= 0)
        {
            yield return AnimationManager.CallBlinkAnimation(QuestionsAreOverImage.gameObject);
        }
        else if (answerResult)
        {
            yield return StartCoroutine(ShowNextQuestion());

            yield break;
        }
        else if (_haveSecondChance)
        {
            //
            _haveSecondChance = false;

            yield return AnimationManager.CallSecondChance(TimerSlider);

            if (!TimerSlider.GetComponent<TimerSlider>().IsTimeOver)
            {
                AdsManager.ShowRewardedAds();
                yield return new WaitForSeconds(.3f); //Waiting for the launch of advertising

                yield return StartCoroutine(ShowNextQuestion());

                yield break;
            }
            //
        }

        yield return AnimationManager.CallBlinkAnimation(CorrectAnswersNumberText.gameObject);

        yield return AnimationManager.CallEndAnimation();

        //Ads
        AdsManager.HideBannerAd();

        StartGameButton.interactable = true;

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
            CorrectAnswersNumberText.text = _correctAnswersNumber.ToString();

            ResultImage.sprite = RightAnswerSprite;
            ResultText.text = "ПРАВИЛЬНЫЙ ОТВЕТ";
        }
        else
        {
            ResultImage.sprite = WrongAnswerSprite;
            ResultText.text = "НЕПРАВИЛЬНЫЙ ОТВЕТ";
        }
    }

    private IEnumerator ShowNextQuestion()
    {
        DetermineQuestion();

        yield return AnimationManager.CallOpenAnimation(QuestionText.gameObject);
        yield return AnimationManager.CallOpenAnimation(AnswerButtons);

        yield break;
    }
}