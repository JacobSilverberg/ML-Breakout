using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingArea : MonoBehaviour
{

    [Tooltip("The agent inside the area")]
    public PaddleAgent paddleAgent;

    [Tooltip("The baby penguin inside the area")]
    public GameObject ball;

    [Tooltip("Prefab of a live fish")]
    public Brick brickPrefab;
    public BrickSet bricksetPrefab;

    /// <summary> 
    /// Resets the learning area back to defualts  
    /// </summary>
    public void ResetArea()
    {
        RemoveAllBricks();
        PlacePaddle();
        PlaceBall();
        PlaceBricks();
    }

    /// <summary> 
    /// Returns the number of bricks left in the simulation
    /// </summary>
    public int BricksRemaining()
    {
        return bricksetPrefab.transform.childCount;
    }

    private void RemoveAllBricks()
    { 

    } 
    
    private void PlacePaddle()
    {
    
            Rigidbody rigidBody = paddleAgent.GetComponent<Rigidbody>();        
            rigidBody.velocity = Vector3.zero;   
            rigidBody.angularVelocity = Vector3.zero;   

            
            paddleAgent.transform.position = new Vector3(Random.Range(-13f, 49.90f), -3.31f);
            //paddleAgent.transform.localPosition  = new Vector3(Random.Range(-13f, 49.90f), -3.31f);
 
    }

    private void PlaceBall()
    {
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();       
        rigidbody.velocity = Vector3.zero;
        ball.transform.position = new Vector3(Random.Range(-25, 25f), 17.89f, 0);
        rigidbody.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

    }
    
    private void PlaceBricks()
    {

    }


    private void Start()
    {
        ResetArea(); 
    }


    private void Update()
    {
        
    }




}















 

    
