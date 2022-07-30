using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_train : MonoBehaviour
{
    Rigidbody _rigidbody;
    public GameObject leftwall;
    public GameObject rightwall;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        leftwall = GameObject.Find("Wall-left");
        rightwall = GameObject.Find("Wall-right");
    }

    void FixedUpdate()
    {
        if ((Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x + 
        rightwall.transform.localScale.x + transform.localScale.x / 2) > 35)
        {
            //Debug.Log(Input.mousePosition.x);
            Debug.Log("out of right boundary");
        }
        else if ((Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x - 
        rightwall.transform.localScale.x - transform.localScale.x / 2) < -35)
        {
            //Debug.Log(Input.mousePosition.x);
            Debug.Log("out of left boundary");
        }
        else 
        {
            _rigidbody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -17, 0));
        }
    }
}
