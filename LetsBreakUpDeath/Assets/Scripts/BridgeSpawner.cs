using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public int bridgeSegments = 10;
    public GameObject bridgePrefab;
    void Start()
    {
        BridgeSegment bridgeSegmentScript = bridgePrefab.GetComponent<BridgeSegment>();

        float bridgePrefabLength = bridgePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        transform.position = new Vector2(0, 0);

        for (int i = 0; i < bridgeSegments; i++)
        {
            Instantiate(bridgePrefab, new Vector3(bridgePrefabLength * i, transform.position.y, transform.position.z), Quaternion.identity);
            if (i == 0)
            {
                bridgeSegmentScript.isFirst = true;
            }
        }

    }
}
