using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBridgeFallTrigger : MonoBehaviour
{
    public List<GameObject> piecesToFall = new List<GameObject>();
    public float shiverDuration = 0.5f;
    public float shiverAmount = 0.05f;
    public bool autoCollectChildren = true;

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

        // 🔄 Shivering phase
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

        // Reset to original position
        for (int i = 0; i < piecesToFall.Count; i++)
        {
            piecesToFall[i].transform.position = originalPositions[i];
        }

        // ✅ SHAKE THE CAMERA!
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(0.3f, 0.2f); // adjust for power
        }

        // ⬇️ Start the fall
        for (int i = 0; i < piecesToFall.Count; i++)
        {
            GameObject block = piecesToFall[i];

            Renderer rend = block.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = Color.grey;

            if (block.GetComponent<Collider2D>() == null)
                block.AddComponent<BoxCollider2D>();

            Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = block.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
