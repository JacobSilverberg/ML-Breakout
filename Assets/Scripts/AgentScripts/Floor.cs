using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    private GameObject ball;
    TrainingArea trainingArea;
    PlayerAgent agent;

    private void Start()
    {
        trainingArea = FindObjectOfType<TrainingArea>();  
        agent = FindObjectOfType<PlayerAgent>();

    }


    private void OnTriggerEnter(Collider collision)
    {

        if (collision.transform.tag == "Ball")
        {
            ball = trainingArea.GetBall();
            ball.GetComponent<TrainingBall>().LostBall(); 
            transform.parent.parent.GetComponent<TrainingArea>().CollisionDetected();
        }
    }
}
