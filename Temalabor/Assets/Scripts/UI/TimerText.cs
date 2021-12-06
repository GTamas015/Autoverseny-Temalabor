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

    public static String timeText;

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
        timeText = TextTimer.text;
        }

    public void StartTimer()
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
