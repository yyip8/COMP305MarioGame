using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject player; // Player reference
    public GameObject groundPrefab1; // Ground prefab reference
    public float ground1Freq;
    public GameObject groundPrefab2; // Ground prefab reference
    public float ground2Freq;
    public GameObject groundPrefab3; // Ground prefab reference
    public float ground3Freq;
    public float groundWidth = 10.0f; // The width of the ground prefab. You need to adjust this depending on your ground prefab size
    public float groundY = -5.5f; // The width of the ground prefab. You need to adjust this depending on your ground prefab size

    private List<GameObject> grounds = new List<GameObject>();
    private float nextSpawnPointRight = 0;
    private float nextSpawnPointLeft = 0;
    private float groundDestroyPointX;

    private void Start()
    {
        // Spawn initial ground
        nextSpawnPointRight = 0;
        SpawnGroundAtRight();
        nextSpawnPointLeft = 0;
        SpawnGroundAtLeft();

        // put the player at x=0 and above ground
        player.transform.position = new Vector3(0, groundY + 5, 0);
    }

    private void Update()
    {
        // Check player position and spawn more ground if necessary
        if (player.transform.position.x > nextSpawnPointRight - 3)
        {
            SpawnGroundAtRight();
        }
        if (player.transform.position.x < nextSpawnPointLeft + 3)
        {
            SpawnGroundAtLeft();
        }
    }

    private void SpawnGroundAtRight()
    {
        float randomNumber = UnityEngine.Random.Range(0, ground1Freq + ground2Freq + ground3Freq);

        Vector3 spawnPosition = new Vector3(nextSpawnPointRight + groundWidth, groundY, 0.0f);

        GameObject newGround;

        if (randomNumber < ground1Freq)
        {
            newGround = Instantiate(groundPrefab1, spawnPosition, Quaternion.identity);
        }
        else if (ground1Freq <= randomNumber && randomNumber < ground2Freq)
        {
            newGround = Instantiate(groundPrefab2, spawnPosition, Quaternion.identity);
        }
        else
        {
            newGround = Instantiate(groundPrefab3, spawnPosition, Quaternion.identity);
        }

        // Update the next spawn point
        grounds.Add(newGround);
        nextSpawnPointRight += groundWidth;
    }

    private void SpawnGroundAtLeft()
    {
        float randomNumber = UnityEngine.Random.Range(0, ground1Freq + ground2Freq + ground3Freq);

        Vector3 spawnPosition = new Vector3(nextSpawnPointLeft, groundY, 0.0f);

        GameObject newGround;

        if (randomNumber < ground1Freq)
        {
            newGround = Instantiate(groundPrefab1, spawnPosition, Quaternion.identity);
        }
        else if (ground1Freq <= randomNumber && randomNumber < ground2Freq)
        {
            newGround = Instantiate(groundPrefab2, spawnPosition, Quaternion.identity);
        }
        else
        {
            newGround = Instantiate(groundPrefab3, spawnPosition, Quaternion.identity);
        }

        // Update the next spawn point
        grounds.Add(newGround);
        nextSpawnPointLeft -= groundWidth;
    }

}
