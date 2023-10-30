using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtEmptyPosition : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject objectToSpawn;    // The object to spawn
   // public Transform spawnPoint, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5;
    public Transform[] spawnPoints; // The empty GameObject where the object will spawn
    // Update is called once per frame
    void Awake()
    {

        // Choose a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomIndex];

        // Spawn an object at the random spawn point
        //SpawnObject(randomSpawnPoint);

    }
    /*
    void SpawnObject()
    {
        if (objectToSpawn != null && spawnPoints != null)
        {
            
            // Get the position of the empty GameObject
            Vector3 spawnPosition = spawnPoints.position;

            // Create a new instance of the object at the spawn position
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Object to spawn or spawn point is not assigned.");
        }
    }
    void SpawnObject(Transform spawnPoint)
    {
        Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    */
}
