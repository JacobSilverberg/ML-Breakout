using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick2P_1_train : MonoBehaviour
{
    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    // Private variables to store original brick material and renderer
    Material _orgMaterial;
    Renderer _renderer;

    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);

        // Get renderer and store original material
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }


    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        hits--;
        if (hits <= 0)
        {
            GameManagerTrain.Instance.Score1 += points;
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
