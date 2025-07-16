using UnityEngine;

public class BridgeTriggerZone : MonoBehaviour
{
    public AutoBlockFallController bridgeScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bridgeScript != null)
        {
            bridgeScript.TriggerBridgeFall();
        }
    }
}
