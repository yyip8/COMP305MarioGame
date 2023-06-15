using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject coinPrefab;
    public float minDistance = -5f;
    public float maxDistance = 5f;
    public float minHeight = 2f;
    public float maxHeight = 10f;
    public float despawnDistance = 30.0f;
    public LayerMask obstacleLayers;
    public float spawnFrequency = 5f;

    private List<GameObject> activeObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        while (true)
        {
            Vector3 spawnPosition = FindSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject obj = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                activeObjects.Add(obj);
            }
            for (int i = activeObjects.Count - 1; i >= 0; i--)
            {
                GameObject obj = activeObjects[i];
                float distance = Vector3.Distance(
                    player.transform.position,
                    obj.transform.position
                );

                // If object is too far from the player, destroy it
                if (distance > despawnDistance)
                {
                    Destroy(obj);
                    activeObjects.RemoveAt(i);
                }
            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    private Vector3 FindSpawnPosition()
    {
        Vector3 spawnPosition = player.transform.position;
        spawnPosition.x += UnityEngine.Random.Range(minDistance, maxDistance);
        spawnPosition.y += UnityEngine.Random.Range(minHeight, maxHeight);

        // Check if there is an object at the spawn position
        if (!Physics2D.OverlapCircle(spawnPosition, 1f, obstacleLayers))
        {
            return spawnPosition;
        }

        return Vector3.zero;
    }
}
