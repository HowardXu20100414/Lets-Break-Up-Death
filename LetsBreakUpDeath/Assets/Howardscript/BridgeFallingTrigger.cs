using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeFallTrigger : MonoBehaviour
{
    public float delayBetweenPieces = 0.2f;
    private bool triggered = false;
    private List<BridgeFallingPiece> bridgePieces;

    void Awake()
    {
        // Find and disable all bridge pieces *before* their Start() runs
        bridgePieces = FindObjectsOfType<BridgeFallingPiece>().ToList();
        foreach (var piece in bridgePieces)
        {
            piece.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(StartBridgeFall());
        }
    }

    IEnumerator StartBridgeFall()
    {
        // Sort left to right
        var sorted = bridgePieces.OrderBy(p => p.transform.position.x).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            // Enable the script (this will now run Start() on it)
            sorted[i].enabled = true;

            // Wait for it to run Start(), assign its index, then let it fall naturally
            yield return new WaitForSeconds(delayBetweenPieces);
        }
    }
}
