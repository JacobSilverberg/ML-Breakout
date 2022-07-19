using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingArea : MonoBehaviour
{

    [Tooltip("The agent inside the training area")]
    public GameObject paddleAgent;

    [Tooltip("The ball to be used in training ")]
    public Ball ballPrefab;  

    [Tooltip("Prefab of the brick set")]
    public BrickSet brickSetPrefab;

    [Tooltip("Reward text to disaply training")]
    public TextMeshPro CumulativeReward;

    [HideInInspector]
    public GameObject ball;

    [HideInInspector]
    public GameObject brickset;
    float _speed = 10f;

    private void Start() {
        //InitialSet();
    }
    private void Update()
    {
        CumulativeReward.text = paddleAgent.GetComponent<PlayerAgent>().GetCumulativeReward().ToString("0.00");
    }

    /// <summary> 
    /// Resets the learning area back to defualts  
    /// </summary>
    public void InitialSet()
    {
        PlaceBricks();
        PlaceBall();
        PlacePaddle();
    } 

    public void ResetArea()
    {
        RemoveObjects();
        ResetObjects();
    }

    public void ResetObjects()
    {
        PlaceBricks();
        PlaceBall();
        PlacePaddle();
    }

    public void RemoveObjects()
    {

        DestroyBricks();
        DestroyBall();

    } 

    
    private void PlacePaddle()
    { 

        Rigidbody rigidBody = paddleAgent.GetComponent<Rigidbody>();        
        rigidBody.velocity = Vector3.zero;   
        rigidBody.angularVelocity = Vector3.zero;   
        
        paddleAgent.transform.position = new Vector3(Random.Range(-13f, 49.90f), -3.31f, 0f);
        paddleAgent.transform.SetParent(transform);

 
    }

    private void PlaceBall()
    { 
        ball = Instantiate(ballPrefab.gameObject);
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();       
        rigidbody.velocity = Vector3.zero;
        ball.transform.position = new Vector3(Random.Range(-25, 25f), 17.89f, 0);
        rigidbody.transform.rotation = Quaternion.Euler(0f, 180f, 0f); 
        rigidbody.velocity = Vector3.down * (_speed - 5f); 

        ball.transform.SetParent(transform, false);

    }
    
    private void PlaceBricks()
    {
        brickset = Instantiate(brickSetPrefab.gameObject);
        brickset.transform.position = new Vector3(-0.4f, 1.62f, 0f);

        brickset.transform.SetParent(transform, false);
    }

    public void DestroyBricks()
    {
        Destroy(brickset);
        brickset = null;

    } 

    private void DestroyBall()
    {
        Destroy(ball);
        ball = null;
    } 

    public GameObject GetBall()
    {

        return ball;

    } 
     
     public void CollisionDetected()
     {
        paddleAgent.GetComponent<PlayerAgent>().NotifyAgentofLostBall(); 
     } 


}















 

    
