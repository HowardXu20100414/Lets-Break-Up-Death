using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject bridgePrefab;
    public GameObject[] carPrefabs;
    float bridgeTopY = -7.5f;
    float spawnRate;
    float spawnTimer;
    float minSpawnRate = 1f;
    float maxSpawnRate = 2f;
    void Start()
    {

        SetNewSpawnRate();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            GameObject randomCar = carPrefabs[Random.Range(0, carPrefabs.Length)];

            float carHeight = randomCar.GetComponent<SpriteRenderer>().bounds.size.y;

            float spawnY = bridgeTopY + carHeight / 2f - 0.15f; //offset so the bottom of the car lines up with the top of the bridge

            Instantiate(randomCar, new Vector2(transform.position.x, spawnY), Quaternion.identity);

            SetNewSpawnRate();
        }
    }

    void SetNewSpawnRate()
    {
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        spawnTimer = spawnRate;
    }
}
