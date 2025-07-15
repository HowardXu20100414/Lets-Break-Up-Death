using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BridgeTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Player entered bridge trigger. Initiating fall sequence for all segments.");

            BridgeSegment[] allBridgeSegments = FindObjectsOfType<BridgeSegment>();

            foreach (BridgeSegment segment in allBridgeSegments)
            {
                segment.AssignFallIndex();
                segment.TriggerFallSequence();
            }
        }
    }
}