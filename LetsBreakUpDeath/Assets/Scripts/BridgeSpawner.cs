using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public int bridgeSegments = 10;
    public GameObject bridgePrefab;
    void Start()
    {
        transform.position = new Vector2(-10, 0);

        BridgeSegment bridgeScript = bridgePrefab.GetComponent<BridgeSegment>();
        float bridgePrefabLength = bridgePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float length;
        if (bridgeScript.fallIndex % 2 == 1)
        {
            length = bridgePrefabLength - 0.3f;
        }
        else
        {
            length = bridgePrefabLength - 0.5f;
        }

        for (int i = 0; i < bridgeSegments; i++)
        {
            Instantiate(bridgePrefab, new Vector3(length * i, transform.position.y, transform.position.z), Quaternion.identity);
        }

    }
}
