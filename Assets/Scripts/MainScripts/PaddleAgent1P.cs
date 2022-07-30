using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgent1P : Agent //observe paddle and ball position, has been trained for 5M steps, a potential successful model
{
    float paddleY = -17f;
    float moveSpeed = 50f;

    private int rewardHitBrick;
    private int rewardHitBall;
    private int levelComplete;
    private int _prevBall = -1; //initialize to -1
    private int _currBall;
    private int _prevScore = 0; //initialize to 0, to avoid lower than initial current score
    private int _currScore;
    private bool brickhit;

    GameObject currentBall;

    public Transform targetTransform;
    public Rigidbody _ballRigidBody;
    public Transform leftWallTransform;
    public Transform rightWallTransform;

    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {
        rewardHitBrick = 5;
        rewardHitBall = 5;
        levelComplete = 100;
        brickhit = false;
        targetTransform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localPosition = new Vector3(0f, paddleY, 0f);
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
            // Keep in the right wall
            if (transform.localPosition.x + (1 * Time.deltaTime * moveSpeed) + rightWallTransform.localScale.x + transform.localScale.x / 2 > 35 - 0.5)
            {
                Debug.Log("out of right boundary");
            }
            else
            {
                transform.localPosition += new Vector3(1 * Time.deltaTime * moveSpeed, 0, 0);
            }
        }
        else if (moveX == 2)
        {
            // Keep in the left wall
            if (transform.localPosition.x + (-1 * Time.deltaTime * moveSpeed) + (-1 * rightWallTransform.localScale.x) + (-1 * transform.localScale.x / 2) < -35 + 0.5)
            {
                Debug.Log("out of left boundary");
            }
            else
            {
                transform.localPosition += new Vector3(-1 * Time.deltaTime * moveSpeed, 0, 0);
            }
        }
        else
        {
            transform.localPosition += new Vector3(0 * Time.deltaTime * moveSpeed, 0, 0);
        }

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y, transform.position.z);
    }

    // Agent Observation
    public override void CollectObservations(VectorSensor sensor)
    {
        //get currentBall reference
        currentBall = getCurrentBall_1P();
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(leftWallTransform.localPosition.x);
        sensor.AddObservation(rightWallTransform.localPosition.x);
        if (currentBall != null)
        {
            sensor.AddObservation(currentBall.GetComponent<Rigidbody>().velocity);
            sensor.AddObservation(currentBall.transform.localPosition.x);
            sensor.AddObservation(currentBall.transform.localPosition.y);
            //observe the x value difference between ball and walls
            sensor.AddObservation(currentBall.transform.localPosition.x - leftWallTransform.localPosition.x);
            sensor.AddObservation(currentBall.transform.localPosition.x - rightWallTransform.localPosition.x);
        }
    }


    /*
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetAxisRaw("Horizontal");
    }*/


    // ##############################################################
    // Reward System
    // ############################################################## 

    //catch a ball with increased reward, reset hitbrick reward
    private void OnCollisionEnter(Collision obj)
    {
        //Debug.Log("Collision: " + obj.gameObject.name);
        if (obj.gameObject == currentBall)
        {        
            // add ballhit condition to avoid AI get reward by hitting ball without hitting any brick
            if (brickhit == true)
            {
                brickhit = false;
                AddReward(rewardHitBall); //reward for catching the ball
                Debug.Log("Positive Reward - catch the ball");
            }
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("level: " + GameManager.Instance.levelCompleted);
        _currBall = GameManager.Instance.Balls;
        _currScore = GameManager.Instance.Score;

        // Ball drops out of game
        if (_currBall < _prevBall && _currBall == 0)
        {
            EndEpisode();
            Debug.Log("Episode Num " + CompletedEpisodes);
            //Debug.Log("Negative Reward - drop a ball");
            //AddReward(-100);
        }

        // hit a brick with increased reward
        if (_currScore > _prevScore)
        {
            Debug.Log("Positive Reward - hit a brick");
            //rewardHitBrick += 5;
            AddReward(rewardHitBrick);
            Debug.Log("Got Reward: " + rewardHitBrick);
            brickhit = true;
        }

        // reward for level completed
        if (GameManager.Instance.levelCompleted == true)
        {
            GameManager.Instance.levelCompleted = false;
            Debug.Log("level: " + GameManager.Instance.levelCompleted);
            AddReward(levelComplete);
        }
        _prevBall = _currBall;
        _prevScore = _currScore;
    }

    private GameObject getCurrentBall_1P()
    {
        currentBall = GameManager.Instance.currentBall;
        return currentBall;
    }
}