using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2P_1 : MonoBehaviour
{
    public float speed1 = 0.1f;
    public GameObject leftwall;
    public GameObject middlewall;
    Rigidbody _rigidbody;
    Vector3 _velocity;
    Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move with A/D keyboard inputs using Horizontal Input
        float xDirection = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(xDirection, 0.0f);
        transform.position += moveDirection * speed1;
    }


    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }

}
