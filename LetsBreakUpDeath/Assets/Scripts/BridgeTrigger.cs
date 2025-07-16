using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BridgeTrigger : MonoBehaviour
{
    private bool triggered = false;
    public bool isRandomTrigger = false;
    private BridgeSegment segment;
    private void Awake()
    {
        if (isRandomTrigger)
        {
            segment = FindObjectOfType<BridgeSegment>();
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            if (isRandomTrigger)
            {
                StartCoroutine(segment.ShakeAndFall());
            }

            Debug.Log("entered trigger! starting to fall");
            BridgeSegment[] allBridgeSegments = FindObjectsOfType<BridgeSegment>();

            foreach (BridgeSegment segment in allBridgeSegments)
            {
                segment.AssignFallIndex();
                segment.TriggerFallSequence();
            }
        }
    }
}