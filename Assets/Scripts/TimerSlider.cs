using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public float fullTime;
    public GameObject fill;
    public Color startColor, endColor;

    private bool isRunning;
    public bool IsTimerRunning
    {
        get { return isRunning; }
    }
    private bool hasReachedTheEnd;
    public bool HasTimerReachedTheEnd
    {
        get { return hasReachedTheEnd; }
    }

    private Slider slider;
    private float timeLeft;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = fullTime;
    }

    private void OnEnable()
    {
        isRunning = true;
        hasReachedTheEnd = false;
        timeLeft = fullTime;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        slider.value = timeLeft;

        fill.GetComponent<Image>().color = Color.Lerp(startColor, endColor, (fullTime - timeLeft) / fullTime);

        if (timeLeft <= 0)
        {
            hasReachedTheEnd = true;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    public void StopTimerOnClick()
    {
        gameObject.SetActive(false);
    }
}
