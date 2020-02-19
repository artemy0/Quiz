using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public float FullTime;
    public Image Fill;
    public Color StartColor, EndColor;

    public bool IsTimerRunning { get; private set; }
    public bool IsTimeOver { get; private set; }

    private float _timeLeft;
    private Slider _slider;
    private Color _fillAreaColor;

    private void Start()
    {
        _slider = GetComponent<Slider>();

        _slider.maxValue = FullTime;
    }

    private void OnEnable()
    {
        IsTimerRunning = true;
        IsTimeOver = false;
        _timeLeft = FullTime;
    }

    private void OnDisable()
    {
        IsTimerRunning = false;
    }

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        _slider.value = _timeLeft;

        Fill.color = Color.Lerp(StartColor, EndColor, (FullTime - _timeLeft) / FullTime);

        if (_timeLeft <= 0)
        {
            IsTimeOver = true;
            gameObject.SetActive(false);
        }
    }

    public void StopTimerOnClick()
    {
        gameObject.SetActive(false);
    }
}
