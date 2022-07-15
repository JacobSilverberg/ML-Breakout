using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PaddleAgent : Agent
{

    float moveSpeed = 20f;
    float leftLimit = -33f;
    float rightLimit = 33f;
    bool lostBall = false;
    bool bricksSpawned = false;

    Spawner spawner;

    private TrainingArea trainingArea;
    new private Rigidbody rigidbody;
    private GameObject ball;


    public override void Initialize()
    {
        base.Initialize(); 
        trainingArea = GetComponentInParent<TrainingArea>();
        ball = trainingArea.ball;
        rigidbody = GetComponent<Rigidbody>();
    }




    void Start()
    { 
        
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    // Resetting the ball for the new episode
    public override void OnEpisodeBegin()
    {

        trainingArea.ResetArea();


        
        if (bricksSpawned == false)
        {
            bricksSpawned = true;
        } 
        else
        {
            spawner.DestroyBricks(brickset);
        }

        StartCoroutine(start_episode());
        transform.localPosition = new Vector3(Random.Range(-13f, 49.90f), -3.31f);
        brickset = spawner.SpawnBricks();

    }

    IEnumerator start_episode()
    {

        yield return new WaitForSeconds(0.5f);
        lostBall = false;
        ball.transform.position = new Vector3(Random.Range(-25, 25f), 17.89f, 0);
        ball.Launch();
        Debug.Log("Episode Start");
    }



    // Agent Observation
    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(transform.position.x); 
        sensor.AddObservation(transform.position.y); 
        sensor.AddObservation(rigidbody.velocity.x); 
        sensor.AddObservation(rigidbody.velocity.y);

        sensor.AddObservation(ball.transform.position.x);
        sensor.AddObservation(ball.transform.position.y); 


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
    
        // Apply a negative reward for every step to encourgage action
        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
        }


        // ##############################################################
        // Reward System
        // ############################################################## 
        float distanceToBall = Vector3.Distance(this.transform.localPosition, _ballTransform.localPosition);

        if (distanceToBall == 0f)
        {
            AddReward(1.0f);
        }

        else if (this.ballLostCheck())
        {
            Debug.Log("Negative reward imposed");
            AddReward(-10f);
            Debug.Log("Ending Episode");
            EndEpisode();
        }

    }

    public void BrickDestroyed()
    {
        Debug.Log("Broke a brick - adding reward");
        AddReward(1.0f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        int moveAction = 0;  



        ActionSegment<int> continuousActions = actionsOut.DiscreteActions;
        continuousActions[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));


    }

    public void LostBall()
    {
        this.lostBall = true;
    }

    private bool ballLostCheck()
    {
        if (this.lostBall == true)
        {
            Debug.Log("Lost the ball");
        }

        return this.lostBall;
    }



}

