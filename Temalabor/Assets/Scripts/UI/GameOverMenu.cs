using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText ;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;


    public void Start()
    {
        setRank();
        setScore();
        setTime();
    }

    public void setRank()
    {
        rankText.text = "1";
    }

    public void setScore() {
        scoreText.text = "1";
    }

    public void setTime() {
        timeText.text = "1";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
