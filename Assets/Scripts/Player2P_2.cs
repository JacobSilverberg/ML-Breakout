using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2P_2 : MonoBehaviour
{
    public float speed2 = 0.1f;
    public GameObject middlewall;
    public GameObject rightwall;

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
        transform.position += moveDirection * speed2;
    }
}
