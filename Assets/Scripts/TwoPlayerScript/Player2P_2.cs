using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2P_2 : MonoBehaviour
{
    private float xValue;
    public float speed2 = 20f;
    public GameObject middlewall;
    public GameObject rightwall;
    public Transform leftWallTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move with L/R keyboard inputs using Horizontal2 Input
        float xDirection = Input.GetAxis("Horizontal2");
        Vector3 moveDirection = new Vector3(xDirection, 0.0f);
        //transform.position += moveDirection * speed2 * Time.deltaTime;

        // Keep in the left wall
        // paddle position + half wall width + half paddle width
        xValue = (transform.localPosition + moveDirection * speed2 * Time.deltaTime).x + (-1 * leftWallTransform.localScale.x) + (-1 * transform.localScale.x / 2);
        if (xValue < 0 + 0.5)
        {
            Debug.Log("Player 2: out of left boundary");
        }
        else if (xValue <= 35 - 3.5 && xValue >= 0 + 0.5)
        {
            transform.localPosition += moveDirection * speed2 * Time.deltaTime;
        }
        //keep in the right wall
        else if (xValue > 35 - 3.5)
        {
            Debug.Log("Player 2: out of right boundary");
        }
        else
        {
            transform.localPosition += moveDirection * speed2 * Time.deltaTime;
        }
    }
}
