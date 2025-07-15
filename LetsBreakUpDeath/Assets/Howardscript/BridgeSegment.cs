using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeFallingPiece : MonoBehaviour
{
    public float delayBetweenPieces = 1f;
    public float shakeDuration = 0.4f;
    public float shakeStrength = 0.1f;
    public float destroyAfter = 5f;

    public int fallIndex = 0;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void AssignFallIndex()
    {
        BridgeFallingPiece[] allPieces;

        if (transform.parent != null)
        {
            allPieces = transform.parent.GetComponentsInChildren<BridgeFallingPiece>();
        }
        else
        {
            allPieces = FindObjectsOfType<BridgeFallingPiece>();
        }

        var sorted = allPieces.OrderBy(p => p.transform.position.x).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            if (sorted[i] == this)
            {
                fallIndex = i;
                break;
            }
        }
    }

    public void TriggerFallSequence()
    {
        float fallDelay = fallIndex * delayBetweenPieces;
        Invoke(nameof(PreFallShake), fallDelay);
    }

    void PreFallShake()
    {
        StartCoroutine(ShakeAndFall());
    }

    IEnumerator ShakeAndFall()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Mathf.Sin(elapsed * 50f) * shakeStrength;
            float offsetY = Random.Range(-shakeStrength, shakeStrength) * 0.5f;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        Fall();
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyAfter);
    }
}
