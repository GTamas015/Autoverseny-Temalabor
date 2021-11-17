using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] CarController car;

    private int currentLaps = 1;


    void Update()
    {
        lapText.text = currentLaps.ToString();

        if (currentLaps == 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
