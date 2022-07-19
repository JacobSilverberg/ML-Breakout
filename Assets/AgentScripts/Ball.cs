using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Ball : MonoBehaviour
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
        rigidbody = gameObject.GetComponent<Rigidbody>(); 
        renderer = gameObject.GetComponent<Renderer>(); 
        trainingArea = FindObjectOfType<TrainingArea>();
        agent = FindObjectOfType<PlayerAgent>();
        //add some delay between launch
        //Invoke("Launch", 0.5f);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * _speed;
        _velocity = rigidbody.velocity; 

        if (transform.position.x > 200f || transform.position.x < -200f || transform.position.y > 500f || transform.position.y < -500f)
        {
            agent.GetComponent<PlayerAgent>().EndEpisode();
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


