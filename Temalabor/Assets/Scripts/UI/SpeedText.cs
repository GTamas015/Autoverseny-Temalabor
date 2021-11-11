using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedText : MonoBehaviour
{
    [SerializeField] CarController car;
    [SerializeField] TextMeshProUGUI speedText;


    // Start is called before the first frame update
    void Start()
    {
        speedText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = car.velocity.ToString("0.00");
    }
}
