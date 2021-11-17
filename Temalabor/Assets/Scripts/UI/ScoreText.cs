using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString(); 
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        score++;
    }
}
