using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollider : MonoBehaviour
{
    private float speedChange = -100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.GetComponent<CarController>().RB.velocity *= 10;
        }
    }

    

   
}
