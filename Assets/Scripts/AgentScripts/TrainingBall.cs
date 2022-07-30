using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;

public class TrainingBall : MonoBehaviour
{
    float _speed = 10f;
    new private Rigidbody rigidbody;
    new private Renderer renderer;
    Vector3 _velocity;
    bool lost_ball = false;
    TrainingArea trainingArea;
    PlayerAgent agent;

    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>(); 
        renderer = this.gameObject.GetComponent<Renderer>(); 
        trainingArea = this.GetComponentInParent<TrainingArea>();
        agent = FindObjectOfType<PlayerAgent>();
        //add some delay between launch
        //Invoke("Launch", 0.5f);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * _speed;
        _velocity = rigidbody.velocity;

        float x_distance = Math.Abs(transform.transform.localPosition.x - trainingArea.Origin.transform.localPosition.x);
        float y_distance = Math.Abs(transform.transform.localPosition.y - trainingArea.Origin.transform.localPosition.y);

        //Debug.Log("X distance: " + x_distance + " | Y Distance: " + y_distance);

        if (x_distance > 37f || y_distance > 22f) 
        {
            Debug.Log("Ball has been lost");
            trainingArea.DeleteBall();
            //agent.GetComponent<PlayerAgent>().EndEpisode();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    } 

    public void LostBall()
    {
        
        lost_ball = true;

    }

    public bool CheckLostBall()
    {
        return lost_ball;
    }


}


