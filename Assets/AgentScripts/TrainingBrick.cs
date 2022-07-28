using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBrick : MonoBehaviour
{
    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    // Private variables to store original brick material and renderer
    Material _orgMaterial;
    Renderer _renderer;

    PlayerAgent paddleAgent;  

    void Start()
    {

        // Get renderer and store original material
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other) { 

        if (other.transform.tag == "Ball")

        {
            paddleAgent = FindObjectOfType<PlayerAgent>();
            paddleAgent.GetComponent<PlayerAgent>().BrickDestroyed();
        } 
        
        hits--;
        if (hits <= 0) {
            Destroy(gameObject);
        }

        // after ball collision, turn brick to hitMaterial, then call RestoreMaterial after 0.05 seconds
        _renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f); 

    } 

    // function to restore brick to original material
    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
        
}

