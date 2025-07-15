using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject bridgePrefab;
    public GameObject carPrefab;
    float bridgeTopY;
    float spawnRate;
    float spawnTimer;
    float minSpawnRate = 3f;
    float maxSpawnRate = 7f;
    void Start()
    {
        if (bridgePrefab != null)
        {
            bridgeTopY = bridgePrefab.GetComponent<SpriteRenderer>().bounds.max.y;
        }

        SetNewSpawnRate();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            float carHeight = carPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

            float spawnY = bridgeTopY + carHeight / 2f; //offset so the bottom of the car lines up with the top of the bridge

            Instantiate(carPrefab, new Vector2(transform.position.x, spawnY), Quaternion.identity);

            SetNewSpawnRate();
        }
    }

    void SetNewSpawnRate()
    {
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        spawnTimer = spawnRate;
    }
}
