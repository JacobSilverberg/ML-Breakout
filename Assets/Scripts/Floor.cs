using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    PaddleAgent paddleAgent; 

    private void OnTriggerEnter(Collider collision)
    { 
        if (collision.transform.tag == "Ball")
        {
            paddleAgent = FindObjectOfType<PaddleAgent>();
            paddleAgent.GetComponent<PaddleAgent>().LostBall();
        }
    }
}
