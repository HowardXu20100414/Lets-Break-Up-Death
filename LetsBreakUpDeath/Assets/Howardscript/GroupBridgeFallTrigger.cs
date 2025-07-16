using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBridgeFallTrigger : MonoBehaviour
{
    public List<GameObject> piecesToFall = new List<GameObject>();
    public float shiverDuration = 0.7f;
    public float shiverAmount = 0.15f;
    public bool autoCollectChildren = true;

    public PlayerFreeze playerFreezeScript; // Drag your player here (must have PlayerFreeze.cs)
    private bool hasTriggered = false;

    void Start()
    {
        if (autoCollectChildren)
        {
            foreach (Transform child in transform)
            {
                piecesToFall.Add(child.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // Freeze player movement
            if (playerFreezeScript != null)
                playerFreezeScript.Freeze();

            // Trigger zoom shake effect
            if (CameraZoomShake.instance != null)
                CameraZoomShake.instance.TriggerZoomShake();

            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        Vector3[] originalPositions = new Vector3[piecesToFall.Count];

        for (int i = 0; i < piecesToFall.Count; i++)
        {
            originalPositions[i] = piecesToFall[i].transform.position;
        }

        // Shiver blocks
        float timer = 0f;
        while (timer < shiverDuration)
        {
            for (int i = 0; i < piecesToFall.Count; i++)
            {
                float x = Random.Range(-shiverAmount, shiverAmount);
                float y = Random.Range(-shiverAmount, shiverAmount);
                piecesToFall[i].transform.position = originalPositions[i] + new Vector3(x, y, 0);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset position and apply physics
        for (int i = 0; i < piecesToFall.Count; i++)
        {
            GameObject block = piecesToFall[i];
            block.transform.position = originalPositions[i];

            // Turn grey
            Renderer rend = block.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = Color.grey;

            // Ensure collider
            if (block.GetComponent<Collider2D>() == null)
                block.AddComponent<BoxCollider2D>();

            // Add Rigidbody2D to fall
            Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = block.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Wait before unfreezing player
        yield return new WaitForSeconds(0.3f);

        if (playerFreezeScript != null)
            playerFreezeScript.Unfreeze();
    }
}
