using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("UI Elements")]
    public GameObject gamePanel;
    public Text qestionText;
    public Button[] answerButtons;

    private Animator gamePanelAnimator;

    private void Start()
    {
        gamePanelAnimator = gamePanel.GetComponent<Animator>();
    }

    public IEnumerator StartGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetOut");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(OpenAnimation());

        yield break;
    }

    public IEnumerator EndGameAnimation()
    {
        gamePanelAnimator.SetTrigger("GetIn");
        yield return new WaitForSeconds(1f);

        yield break;
    }

    public IEnumerator OpenAnimation()
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

    public IEnumerator CloseAnimation()
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

    public IEnumerator BlinkAnimation(GameObject blinkingUIObject)
    {
        yield return new WaitForSeconds(.2f);

        if (!blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(true);

        blinkingUIObject.GetComponent<Animator>().SetTrigger("Open");

        yield return new WaitForSeconds(1f);

        blinkingUIObject.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        if (blinkingUIObject.activeSelf)
            blinkingUIObject.SetActive(false);

        yield return new WaitForSeconds(.2f);

        yield break;
    }
}
