using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public float bridgeSegmentLength = 7f;
    public int bridgeSegments = 10;
    public GameObject bridgePrefab;
    void Start()
    {
        transform.position = new Vector2(0, 0);

        for (int i = 0; i < bridgeSegments; i++)
        {
            Instantiate(bridgePrefab, new Vector3(bridgeSegmentLength * i, transform.position.y, transform.position.z), Quaternion.identity);
        }

    }
}
