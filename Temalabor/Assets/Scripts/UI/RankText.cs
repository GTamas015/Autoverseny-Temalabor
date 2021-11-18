using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    public int rank = 1;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        rankText.text = rank.ToString();
    }

}
