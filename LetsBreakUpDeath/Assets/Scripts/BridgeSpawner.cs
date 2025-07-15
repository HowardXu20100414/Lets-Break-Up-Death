using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public int bridgeSegments = 10;               // Number of bridge segments
    public GameObject bridgePrefab;               // Prefab for each bridge piece
    public GameObject pillarPrefab;               // Prefab for the supporting pillar
    public GameObject triggerZonePrefab;          // Prefab for the trigger zone

    private List<BridgeFallingPiece> bridgePieces = new List<BridgeFallingPiece>();

    void Start()
    {
        // Check that all prefabs are assigned
        if (bridgePrefab == null)
        {
            Debug.LogError("Bridge prefab is not assigned!");
            return;
        }

        if (pillarPrefab == null)
        {
            Debug.LogError("Pillar prefab is not assigned!");
            return;
        }

        if (triggerZonePrefab == null)
        {
            Debug.LogError("Trigger zone prefab is not assigned!");
            return;
        }

        // Get the width of one bridge piece
        float bridgePrefabLength = bridgePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int i = 0; i < bridgeSegments; i++)
        {
            // Spawn bridge piece
            Vector3 spawnPos = new Vector3(transform.position.x + bridgePrefabLength * i, transform.position.y, transform.position.z);
            GameObject piece = Instantiate(bridgePrefab, spawnPos, Quaternion.identity);
            BridgeFallingPiece pieceScript = piece.GetComponent<BridgeFallingPiece>();
            pieceScript.AssignFallIndex(); // Assign index for sequential fall
            bridgePieces.Add(pieceScript);

            // Place pillar every 5 segments, positioned below the bridge
            if (i % 5 == 0)
            {
                SpriteRenderer pillarRenderer = pillarPrefab.GetComponent<SpriteRenderer>();
                if (pillarRenderer == null)
                {
                    Debug.LogError("Pillar prefab is missing a SpriteRenderer!");
                    continue;
                }

                float pillarHeight = pillarRenderer.bounds.size.y;
                Vector3 pillarPos = new Vector3(spawnPos.x, spawnPos.y - pillarHeight / 2f - 0.1f, spawnPos.z);
                Instantiate(pillarPrefab, pillarPos, Quaternion.identity);
            }
        }

        // Spawn trigger zone just before the bridge starts
        Vector3 triggerPos = transform.position + new Vector3(-1f, 0, 0); // adjust if needed
        GameObject trigger = Instantiate(triggerZonePrefab, triggerPos, Quaternion.identity);

        BridgeTrigger triggerScript = trigger.GetComponent<BridgeTrigger>();
        if (triggerScript != null)
        {
            triggerScript.Setup(this);
        }
        else
        {
            Debug.LogError("Trigger zone prefab is missing the BridgeTrigger script!");
        }
    }

    // Called by trigger when player enters the zone
    public void ActivateBridgeFall()
    {
        StartCoroutine(FallInSequence());
    }

    // Coroutine to trigger each bridge piece to fall one by one
    private IEnumerator FallInSequence()
    {
        for (int i = 0; i < bridgePieces.Count; i++)
        {
            bridgePieces[i].TriggerFallSequence();
            yield return new WaitForSeconds(0.2f); // Delay between each piece
        }
    }
}
