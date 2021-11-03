using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI TextTimer;

    [Header("Logic")]
    private float timer;
    private bool isActive;


    private float elapsedTime;
    private TimeSpan timePlaying;

    public void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            UpdateText();
        }
    }
    private void UpdateText()
    {
        elapsedTime += Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        TextTimer.text = timePlaying.ToString("mm':'ss'.'ff");
        }

    public void Start()
    {
        StartTimer(0);
    }

    public void StartTimer(float f)
    {
        isActive = true;
        timer = f;
        UpdateText();
    }

}
