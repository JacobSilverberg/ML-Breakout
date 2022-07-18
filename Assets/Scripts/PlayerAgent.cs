using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAgent : Agent
{

    float moveSpeed = 20f;
    float leftLimit = -33f;
    float rightLimit = 33f;

    private TrainingArea trainingArea;
    new private Rigidbody rigidbody;
    private GameObject ball;
    private GameObject walls;
    private GameObject rightwall;
    private GameObject leftwall;
    int bricks_available = 266;
    int bricks_destroyed = 0;

    public override void Initialize()
    {

        base.Initialize();
        trainingArea = GetComponentInParent<TrainingArea>();
        rightwall = GameObject.FindGameObjectWithTag("Right_Wall");
        leftwall = GameObject.FindGameObjectWithTag("Left_Wall");
        rigidbody = GetComponent<Rigidbody>();

    }


    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {
        trainingArea.ResetArea();
        bricks_destroyed = 0;
        ball = trainingArea.ball;
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
            transform.localPosition += new Vector3(-1 * Time.deltaTime * moveSpeed, 0, 0);
        }
        else
        {
            transform.localPosition += new Vector3(0 * Time.deltaTime * moveSpeed, 0, 0);
        }

        transform.position =  new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y, transform.position.z);
    


        // ##############################################################
        // Reward System
        // ############################################################## 

        // Apply a negative reward for every step to encourgage action
        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
        }


    }

    // Agent Observation
    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(Vector3.Distance(ball.transform.position, transform.position));
        sensor.AddObservation((ball.transform.position - transform.position).normalized);

        sensor.AddObservation(transform.position.x); 
        sensor.AddObservation(transform.position.y); 
        sensor.AddObservation(rigidbody.velocity.x); 
        sensor.AddObservation(rigidbody.velocity.y);

        sensor.AddObservation(ball.transform.position.x);
        sensor.AddObservation(ball.transform.position.y); 

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> continuousActions = actionsOut.DiscreteActions;
        continuousActions[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            // Add reward for ball 
            AddReward(1f);
        }  
        
    } 

    public void BrickDestroyed()
    {
        AddReward(1.0f);

        bricks_destroyed++; 

        if ((bricks_destroyed / bricks_available) > 0.1f)
        {
            AddReward(10f);
        } 
        else if ((bricks_destroyed / bricks_available) > 0.25)
        {
            AddReward(25f);
        }
        else if ((bricks_destroyed / bricks_available) > 0.50)
        {
            AddReward(50f);
        }
        else if ((bricks_destroyed / bricks_available) > 0.90)
        {
            AddReward(100f);
        }
        else if ((bricks_destroyed / bricks_available) > 0.99)
        {
            AddReward(1000f);
        }

    }
    
    public void NotifyAgentofLostBall()
    {
        EndEpisode(); 
    }

    void FixedUpdate()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0f);
        transform.position += move * moveSpeed * Time.deltaTime;
    }
    
    public float GetReward()
    {
        return GetCumulativeReward();
    }

}

