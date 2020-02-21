using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("UI Elements")]
    public GameObject gamePanel;

    private Animator gamePanelAnimator;

    private void Start()
    {
        gamePanelAnimator = gamePanel.GetComponent<Animator>();
    }

    public IEnumerator StartGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetOut");
        yield return StartCoroutine(WaitCurrentAnimationFinish(gamePanelAnimator));

        yield return StartCoroutine(OpenAnimation(gameManager.questionText.gameObject));
        yield return StartCoroutine(OpenButtonsAnimation(gameManager.answerButtons));

        yield break;
    }

    public IEnumerator EndGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetIn");

        yield return StartCoroutine(WaitCurrentAnimationFinish(gamePanelAnimator));

        yield break;
    }

    public IEnumerator OpenAnimation(GameObject openableObject)
    {
        if (!openableObject.activeSelf)
            openableObject.SetActive(true);

        openableObject.GetComponent<Animator>().SetTrigger("Open");

        yield return StartCoroutine(WaitCurrentAnimationFinish(openableObject.GetComponent<Animator>()));

        yield break;
    }

    public IEnumerator OpenButtonsAnimation(Button[] answerButtons)
    {
        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (!answerButtons[i].gameObject.activeSelf)
                answerButtons[i].gameObject.SetActive(true);

            answerButtons[i].gameObject.GetComponent<Animator>().SetTrigger("Open");

            yield return StartCoroutine(WaitCurrentAnimationFinish(answerButtons[i].gameObject.GetComponent<Animator>()));
        }

        for (int i = 0; i < answerButtons.Length; i++) //buttons can be pressed
            answerButtons[i].interactable = true;

        yield break;
    }

    public IEnumerator CloseAnimation(GameObject lockableObject)
    {
        Animator lockableObjectAnimator = lockableObject.GetComponent<Animator>();
        lockableObjectAnimator.SetTrigger("Close");
        yield return StartCoroutine(WaitCurrentAnimationFinish(lockableObjectAnimator));

        yield break;
    }

    public IEnumerator CloseButtonsAnimation(Button[] answerButtons)
    {
        for (int i = 0; i < answerButtons.Length; i++) //buttons cannot be pressed
            answerButtons[i].interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.GetComponent<Animator>().SetTrigger("Close");
        }

        yield return new WaitForSeconds(.17f);

        yield break;
    }

    public IEnumerator BlinkAnimation(GameObject blinkingUIObject)
    {
        if (!blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(true);

        blinkingUIObject.GetComponent<Animator>().SetTrigger("Open");

        yield return StartCoroutine(WaitCurrentAnimationFinish(blinkingUIObject.GetComponent<Animator>()));

        blinkingUIObject.GetComponent<Animator>().SetTrigger("Close");

        yield return StartCoroutine(WaitCurrentAnimationFinish(blinkingUIObject.GetComponent<Animator>()));

        if (blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(false);

        yield break;
    }

    private IEnumerator WaitCurrentAnimationFinish(Animator animator)
    {
        yield return new WaitForEndOfFrame(); //ждать окончания кадра чтобы анимация успела воспроизвестись

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

}
