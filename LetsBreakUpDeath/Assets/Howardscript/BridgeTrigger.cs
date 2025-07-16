/*using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    private BridgeSpawner spawner;
    private bool triggered = false;

    // Called by the spawner to give this trigger a reference back to it
    public void Setup(BridgeSpawner s)
    {
        spawner = s;
    }

    // Called when any collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger once and only for the player
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            spawner.ActivateBridgeFall();
        }
    }
}*/
