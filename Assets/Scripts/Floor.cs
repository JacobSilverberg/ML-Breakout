using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    PaddleAgent paddleAgent; 


    private void OnTriggerEnter(Collider other)
    { 

        paddleAgent = FindObjectOfType<PaddleAgent>();
        paddleAgent.GetComponent<PaddleAgent>().LostBall();
    }
}
