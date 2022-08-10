using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    public GameObject leftwall;
    public GameObject rightwall;
    private float xValue;
    public float speed1 = 20f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        leftwall = GameObject.Find("Wall-left");
        rightwall = GameObject.Find("Wall-right");
    }

    void FixedUpdate()
    {
        // move with A/D keyboard inputs using Horizontal Input 
        float xDirection = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(xDirection, 0.0f);

        // Keep in the left wall
        // paddle position + half wall width + half paddle width
        xValue = (transform.localPosition + moveDirection * speed1 * Time.deltaTime).x + (-1 * leftwall.transform.localScale.x) + (-1 * transform.localScale.x / 2);

        if (xValue < -35 + 0.5)
        {
            Debug.Log("Player 1: out of left boundary");
        }
        else if (xValue <= 0 - 3.5 && xValue >= -35 + 0.5)
        {
            transform.localPosition += moveDirection * speed1 * Time.deltaTime;
        }
        //keep in the right wall
        else if (xValue > 35 - 0.5)
        {
            Debug.Log("Player 1: out of right boundary");
        }
        else
        { 
            transform.Translate(x: (speed1 * xDirection) * Time.deltaTime, y: 0f, z: 0f);
        }

    }
}
