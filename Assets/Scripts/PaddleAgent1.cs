using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgent1 : Agent //using camera sensor 
{
    float paddleY = -17f;
    float moveSpeed = 50f;
    float leftLimit = -33f;
    float rightLimit = 33f;
    //float lastPositionDifference;
    private int rewardHitBrick;
    private int rewardHitBall;
    private int _prevBall = -1; //initialize to -1
    private int _currBall;
    private int _prevScore = 0; //initialize to 0, to avoid lower than initial current score
    private int _currScore;

    //private int brickTotal = 59; // 59 bricks in level1_easier
    //private int brickTotal = 265; // 265 bricks in level1 

    //private int brickCount = 0; //how many bricks being hit before ball colliding with paddle
    GameObject currentBall;
    //bool sameBallTrip; //mark if events happen in the same ball trip (time between hitting the paddles)
    //bool wallHit = false;
    private bool ballhit;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Rigidbody _ballRigidBody;
    [SerializeField] private Transform leftWallTransform;
    [SerializeField] private Transform rightWallTransform;
    //[SerializeField] private Transform ceilingTransform;

    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {
        rewardHitBrick = 0;
        rewardHitBall = 5;
        ballhit = false;
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
            if (transform.localPosition.x + (1 * Time.deltaTime * moveSpeed) + rightWallTransform.localScale.x + transform.localScale.x / 2 > 35 - 1)
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
            if (transform.localPosition.x + (-1 * Time.deltaTime * moveSpeed) + (-1 * rightWallTransform.localScale.x) + (-1 * transform.localScale.x / 2) < -35 + 1)
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
        currentBall = getCurrentBall();
        sensor.AddObservation(transform.localPosition.x);
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
            if (ballhit == false)
            {
                ballhit = true;
                AddReward(rewardHitBall); //reward for catching the ball
                Debug.Log("Positive Reward - catch the ball");
                Debug.Log("Got Reward: " + rewardHitBall);
                rewardHitBrick = 5; // every time when the ball hit the paddle, the reward starting point will be reset.
                Debug.Log("reward reset: " + rewardHitBrick);
            }
        }
    }

    private void FixedUpdate()
    {
        _currBall = GameManager.Instance.Balls;
        _currScore = GameManager.Instance.Score;
        //var curPositionDifference = Math.Abs(targetTransform.position.x - transform.position.x);
        //Debug.Log(targetTransform.position.x);

        // Ball drops out of game
        if (_currBall < _prevBall && _currBall == 0)
        {
            EndEpisode();
            Debug.Log("Episode Num " + CompletedEpisodes);
            Debug.Log("Negative Reward - drop a ball");
            AddReward(-100);
        }

        //hit a brick with increased reward
        if (_currScore > _prevScore)
        {
            Debug.Log("Positive Reward - hit a brick");
            rewardHitBrick += 5;
            AddReward(rewardHitBrick);
            Debug.Log("Got Reward: " + rewardHitBrick);
            ballhit = false; //reset ballhit value
        }

        /*
        // Paddle is getting closer to the Ball
        if (curPositionDifference < lastPositionDifference)
        {
            AddReward(5f);
            Debug.Log("Positive Reward - position - closer to the ball");
        }

        // Paddle is getting further away from the ball when ball hit the wall
        else if (wallHit == true && (curPositionDifference > lastPositionDifference))
        {
            AddReward(+5f);
            Debug.Log("Positive Reward - position - further to the ball");
        }

        // Paddle is getting further away from the ball when ball doesn't hit the wall
        else if (wallHit == false && (curPositionDifference > lastPositionDifference))
        {
            AddReward(-5f);
            Debug.Log("Negative Reward - position - further to the ball");
        }
        */

        //lastPositionDifference = curPositionDifference;
        _prevBall = _currBall;
        _prevScore = _currScore;
    }

    private GameObject getCurrentBall()
    {
        currentBall = GameManager.Instance.currentBall;
        return currentBall;
    }
}