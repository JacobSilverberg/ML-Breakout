using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgent : Agent
{
    float paddleY = -17f;
    float moveSpeed = 50f;
    float leftLimit = -33f;
    float rightLimit = 33f;


    [SerializeField] private Transform targetTransform;
    [SerializeField] private Rigidbody _ballRigidBody;
    //[SerializeField] public Transform wallTransform;

    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode Start");
        targetTransform.position = new Vector3(0f, paddleY, 0f);
        //Debug.Log("ball launched");
        _ballRigidBody.velocity = Vector3.down * moveSpeed;
    }

    // Agent Action
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        //Debug.Log(moveX);

        // Move paddle right, left, or hold
        if (moveX == 1)
        {
            transform.localPosition += new Vector3(1 * Time.deltaTime * moveSpeed, 0, 0);
        }
        else if (moveX == 2)
        {
            transform.localPosition += new Vector3(-1 * Time.deltaTime * moveSpeed, 0, 0);
        }
        else
        {
            transform.localPosition += new Vector3(0 * Time.deltaTime * moveSpeed, 0, 0);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y, transform.position.z);

        
        /*// Make sure paddle is within walls
        if (transform.localPosition.x < 35 && transform.localPosition.x > -35){
            transform.localPosition += new Vector3(moveX, 0, 0) * Time.deltaTime * moveSpeed;
        }*/
       
    }

    
    // Agent Observation
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(targetTransform.localPosition.x);
        //sensor.AddObservation()
        //sensor.AddObservation(transform.localPosition); // This is the 'transform' localPosition of the agent object, the paddle
        //sensor.AddObservation(targetTransform.localPosition); // This is the localPosition of a target which is the ball

    }

    /*
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }*/
    

    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.name == "Ball")
        {
            Debug.Log("Positive Reward");
            AddReward(10f);
        }
    }


    float lastPositionDifference;
    //initialize to -1
    private int _prevBall = -1;
    private int _currBall;
    private int _prevScore = -1;
    private int _currScore;

    private void FixedUpdate()
    {
        //Debug.Log("prev" + _prevBall);
        _currBall = GameManager.Instance.Balls;
        var curPositionDifference = Math.Abs(targetTransform.position.x - transform.position.x);
        //Debug.Log(targetTransform.position.x);

        
        // Ball drops out of game
        //Debug.Log("curr" + _currBall);
        if (_currBall < _prevBall)
        //if (targetTransform.transform.localPosition.y < -18f)
        {
            Debug.Log("Negative Reward - drop a ball");
            AddReward(-100f);
            //Debug.Log("Episode End");
            EndEpisode();
        }

        _currScore = GameManager.Instance.Score;
        //Debug.Log(_currScore);
        if (_currScore > _prevScore)
        {
            AddReward(50f);
            Debug.Log("Positive Reward - hit a brick");
        }


        // Paddle is getting closer to the Ball
        if (curPositionDifference < lastPositionDifference)
        {
            AddReward(5f);
            Debug.Log("Positive Reward - position");
        }

        // Paddle is getting further away from the ball
        else if (curPositionDifference > lastPositionDifference)
        {
            AddReward(-5f);
            Debug.Log("Negative Reward - position");
        }


        lastPositionDifference = curPositionDifference;
        _prevBall = _currBall;
        _prevScore = _currScore;
    }
}