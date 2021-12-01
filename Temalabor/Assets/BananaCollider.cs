using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("start");
            other.GetComponent<CarController>().bananaEffect();
            Debug.Log("end");
        }
    }

    

   
}
