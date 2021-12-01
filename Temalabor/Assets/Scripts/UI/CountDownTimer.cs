using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] CarController car;
    [SerializeField] EnemyControls enemy1;
    [SerializeField] EnemyControls enemy2;
    [SerializeField] Text countdownText;
    [SerializeField] TimerText timer;
    [SerializeField]  int countTime;

    
    void Start()
    {
        StartCoroutine(countDownToStart());
    }

    IEnumerator countDownToStart()
    {
        while (countTime > 0)
        {
            car.enabled = false;
            enemy1.enabled = false;
            enemy2.enabled = false;
            countdownText.text = countTime.ToString();

            yield return new WaitForSeconds(1f);

            countTime--;
        }

        countdownText.text = "GO!";
        car.enabled = true;
        enemy1.enabled = true;
        enemy2.enabled = true;
        timer.StartTimer();
        yield return new WaitForSeconds(2);
        countdownText.gameObject.SetActive(false);
    }

   

}
