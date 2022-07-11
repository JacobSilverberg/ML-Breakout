using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgent : Agent
{

    float moveSpeed = 20f;
    float leftLimit = -33f;
    float rightLimit = 33f;   
    bool lostBall = false;

    // Ball transforms
    [SerializeField] private Transform _ballTransform;
    [SerializeField] private Rigidbody _ballRigidBody;
        
    //[SerializeField] private Transform _bricksTransform;
    //[SerializeField] private Rigidbody _bricksRigidBody; 


    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {
        //Debug.Log("Episode Start");
        _ballTransform.position = new Vector3(0, 17.89f, 0);
        _ballRigidBody.velocity = Vector3.down * moveSpeed; 
        
    }

    // Agent Action
    public override void OnActionReceived(ActionBuffers actions)
    { 
        // ##############################################################
        // Action System
        // ##############################################################
        float moveX = actions.DiscreteActions[0];

        //Debug.Log("Moving agent in: " + actions.DiscreteActions[0]);

        // Move paddle right, left, or hold
        if (moveX == 1)
        {
            transform.localPosition += new Vector3(1 * Time.deltaTime * moveSpeed, 0, 0);
        }
        else if (moveX == 2)
        {
            transform.localPosition += new Vector3(-1  * Time.deltaTime * moveSpeed, 0, 0);
        }
        else
        {
            transform.localPosition += new Vector3(0 * Time.deltaTime * moveSpeed, 0, 0);
        }
    
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y, transform.position.z);

        /*
        // Make sure paddle is within walls
        if (transform.localPosition.x < 35 && transform.localPosition.x > -35){
            transform.localPosition += new Vector3(moveX, 0, 0) * Time.deltaTime * moveSpeed;
        }
        */

        // ##############################################################
        // Reward System
        // ############################################################## 
        float distanceToBall = Vector3.Distance(this.transform.localPosition, _ballTransform.localPosition);

        if (distanceToBall == 0f)
        {
            //SetReward(10f);
        }

        else if (this.ballLostCheck())
        {
            SetReward(-10f);
            EndEpisode();
        }

    }

    // Agent Observation
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(_ballTransform.localPosition.x);
        //sensor.AddObservation(transform.localPosition); // This is the 'transform' localPosition of the agent object, the paddle
        //sensor.AddObservation(targetTransform.localPosition); // This is the localPosition of a target which is the ball
 
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> continuousActions = actionsOut.DiscreteActions;
        continuousActions[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
    }

    public void LostBall()
    {
        this.lostBall = true;
    }

    private bool ballLostCheck()
    {
        if (this.lostBall == true) {
            Debug.Log("Lost the ball");
        } 

        return this.lostBall; 
    }


    
}

