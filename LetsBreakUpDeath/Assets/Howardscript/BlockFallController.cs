using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlockFallController : MonoBehaviour
{
    public float delayBetweenBlocks = 0.5f;
    public float shiverDuration = 0.5f;
    public float shiverAmount = 0.05f;

    private List<Transform> blocks = new List<Transform>();
    private bool hasStarted = false;

    void Awake()
    {
        // Collect all child blocks
        foreach (Transform child in transform)
        {
            blocks.Add(child);

            // Ensure block has collider
            if (child.GetComponent<Collider2D>() == null)
                child.gameObject.AddComponent<BoxCollider2D>();

            // Remove any Rigidbody2D (start frozen)
            var rb = child.GetComponent<Rigidbody2D>();
            if (rb != null)
                Destroy(rb);
        }

        // Sort left to right by x position
        blocks.Sort((a, b) => a.position.x.CompareTo(b.position.x));
    }

    // Called by the trigger script
    public void TriggerBridgeFall()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(StartFallingSequence());
        }
    }

    IEnumerator StartFallingSequence()
    {
        foreach (Transform block in blocks)
        {
            yield return StartCoroutine(ShiverAndFall(block.gameObject));
            yield return new WaitForSeconds(delayBetweenBlocks);
        }
    }

    IEnumerator ShiverAndFall(GameObject block)
    {
        Vector3 originalPos = block.transform.position;

        float timer = 0f;
        while (timer < shiverDuration)
        {
            float x = Random.Range(-shiverAmount, shiverAmount);
            float y = Random.Range(-shiverAmount, shiverAmount);
            block.transform.position = originalPos + new Vector3(x, y, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        block.transform.position = originalPos;

        Renderer rend = block.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.grey;

        Rigidbody2D rb = block.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
