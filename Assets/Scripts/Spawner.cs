using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject BrickSpawner;

    public GameObject SpawnBricks()
    {
        Debug.Log("Spawning Bricks");
        GameObject newSpawnItem = Instantiate(BrickSpawner);

        return newSpawnItem;
    }

    public void DestroyBricks(GameObject destroyItem)
    {

        Destroy(destroyItem);

    } 


}
