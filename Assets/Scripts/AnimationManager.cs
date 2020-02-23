using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    //Самый простой, как мне кажется, вариант это не создавать несколько аниматоров (оотдельный для каждей кнопки), а
    //создать один аниматор который управлял бы всеми анимациями (код получился бы четабельнее, а работа удобнее), но
    //когда я создавал эту игру я был зелёным маслёнкам, посмотрел видос на youtube и, собственно, вот. 
    //P.S. знаю что это не оправдание и постараюсь это переделать specially for Artyom from the future.

    private Animator _gamePanelAnimator;

    private void Start()
    {
        _gamePanelAnimator = gameObject.GetComponent<Animator>();
    }

    public Coroutine CallSecondChance(Button continueGameButton, TimerSlider timerSlider)
    {
        return StartCoroutine(SecondChance(continueGameButton, timerSlider));
    }

    public Coroutine CallStartAnimation(Text questionText, Button[] answerButtons)
    {
        return StartCoroutine(StartAnimation(questionText, answerButtons));
    }

    public Coroutine CallEndAnimation()
    {
        return StartCoroutine(EndAnimation());
    }

    public Coroutine CallOpenAnimation(GameObject openableObject)
    {
        return StartCoroutine(OpenAnimation(openableObject));
    }

    public Coroutine CallOpenAnimation(Button[] answerButtons)
    {
        return StartCoroutine(OpenAnimation(answerButtons));
    }

    public Coroutine CallCloseAnimation(GameObject lockableObject)
    {
        return StartCoroutine(CloseAnimation(lockableObject));
    }

    public Coroutine CallCloseAnimation(Button[] answerButtons)
    {
        return StartCoroutine(CloseAnimation(answerButtons));
    }

    public Coroutine CallBlinkAnimation(GameObject blinkingUIObject)
    {
        return StartCoroutine(BlinkAnimation(blinkingUIObject));
    }


    private IEnumerator WaitCurrentAnimationFinish(Animator animator)
    {
        yield return new WaitForEndOfFrame(); //ждать окончания кадра чтобы анимация успела воспроизвестись
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private IEnumerator StartAnimation(Text questionText, Button[] answerButtons)
    {
        _gamePanelAnimator.SetTrigger("GetOut");
        yield return StartCoroutine(WaitCurrentAnimationFinish(_gamePanelAnimator));

        yield return StartCoroutine(OpenAnimation(questionText.gameObject));
        yield return StartCoroutine(OpenAnimation(answerButtons));

        yield break;
    }

    private IEnumerator EndAnimation()
    {
        _gamePanelAnimator.SetTrigger("GetIn");
        yield return StartCoroutine(WaitCurrentAnimationFinish(_gamePanelAnimator));

        yield break;
    }

    private IEnumerator OpenAnimation(GameObject openableObject)
    {
        if (!openableObject.activeSelf)
            openableObject.SetActive(true);

        Animator openableObjectAnimator = openableObject.GetComponent<Animator>();
        openableObjectAnimator.SetTrigger("Open");
        yield return StartCoroutine(WaitCurrentAnimationFinish(openableObjectAnimator));

        yield break;
    }

    private IEnumerator OpenAnimation(Button[] answerButtons)
    {
        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (!answerButtons[i].gameObject.activeSelf)
                answerButtons[i].gameObject.SetActive(true);

            Animator answerButtonAnimator = answerButtons[i].gameObject.GetComponent<Animator>();
            answerButtonAnimator.SetTrigger("Open");
            yield return StartCoroutine(WaitCurrentAnimationFinish(answerButtonAnimator));
        }

        for (int i = 0; i < answerButtons.Length; i++) //buttons can be pressed
            answerButtons[i].interactable = true;

        yield break;
    }

    private IEnumerator CloseAnimation(GameObject lockableObject)
    {
        Animator lockableObjectAnimator = lockableObject.GetComponent<Animator>();
        lockableObjectAnimator.SetTrigger("Close");
        yield return StartCoroutine(WaitCurrentAnimationFinish(lockableObjectAnimator));

        yield break;
    }

    private IEnumerator CloseAnimation(Button[] answerButtons)
    {
        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            Animator answerButtonAnimator = answerButtons[i].gameObject.GetComponent<Animator>();
            answerButtonAnimator.SetTrigger("Close");
        }

        yield return new WaitForSeconds(.17f);

        yield break;
    }

    private IEnumerator BlinkAnimation(GameObject blinkingUIObject)
    {
        if (!blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(true);

        Animator blinkingUIObjectAnimator = blinkingUIObject.GetComponent<Animator>();
        blinkingUIObjectAnimator.SetTrigger("Open");
        yield return StartCoroutine(WaitCurrentAnimationFinish(blinkingUIObjectAnimator));
        blinkingUIObjectAnimator.SetTrigger("Close");
        yield return StartCoroutine(WaitCurrentAnimationFinish(blinkingUIObjectAnimator));

        if (blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(false);

        yield break;
    }

    private IEnumerator SecondChance(Button continueGameButton, TimerSlider timerSlider)
    {
        yield return CallOpenAnimation(continueGameButton.gameObject); //continueGameButton.gameObject.SetActive(true);
        timerSlider.gameObject.SetActive(true);

        yield return new WaitWhile(() => { return timerSlider.GetComponent<TimerSlider>().IsTimerRunning; });

        timerSlider.gameObject.SetActive(false);
        yield return CallCloseAnimation(continueGameButton.gameObject);
        continueGameButton.gameObject.SetActive(false);
    }
}
