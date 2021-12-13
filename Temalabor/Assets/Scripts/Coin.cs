using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public float rotationspeed = 80;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationspeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy") 
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            ScoreText.score++;
            other.GetComponent<CarController>().coinEffect();
            Destroy(gameObject);
        }
    }
}
