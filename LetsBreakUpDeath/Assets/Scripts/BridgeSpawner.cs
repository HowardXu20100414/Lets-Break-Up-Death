using UnityEngine;
using System.Linq;

public class BridgeSpawner : MonoBehaviour
{
    public int bridgeSegments = 10;
    public GameObject bridgePrefab;
    public GameObject bridgeTriggerPrefab;
    public Vector2 bridgeTriggerLocation = new Vector2(30, 0);

    void Start()
    {
        transform.position = new Vector2(0, 0);

        float bridgePrefabLength = bridgePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float currentXPosition = transform.position.x;

        for (int i = 0; i < bridgeSegments; i++)
        {
            float adjustedSegmentLength = bridgePrefabLength;

            GameObject bridgeObj = Instantiate(bridgePrefab, new Vector3(currentXPosition * 0.9f, transform.position.y, transform.position.z), Quaternion.identity);
            Debug.Log($"Spawned Bridge Segment {i} at {bridgeObj.transform.position}");

            currentXPosition += adjustedSegmentLength;
        }

        GameObject triggerObj = Instantiate(bridgeTriggerPrefab, bridgeTriggerLocation, Quaternion.identity);
        Debug.Log($"Spawned Single Bridge Trigger at {triggerObj.transform.position}");
    }
}