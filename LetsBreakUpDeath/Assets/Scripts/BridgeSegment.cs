using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeSegment : MonoBehaviour
{
    public float delayBetweenPieces = 1f;
    public float shakeDuration = 0.4f;
    public float shakeStrength = 0.1f;
    public float destroyAfter = 5f;

    public int fallIndex = 0;
    public Rigidbody2D rb;

    void Awake()
    {
        Debug.Log("awake");
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void AssignFallIndex()
    {
        BridgeSegment[] allPieces = FindObjectsOfType<BridgeSegment>();
        var sorted = allPieces.OrderBy(p => p.transform.position.x).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            if (sorted[i] == this)
            {
                fallIndex = i;
                break;
            }
        }
        Debug.Log($"{gameObject.name} assigned fallIndex: {fallIndex}");
    }

    public void TriggerFallSequence()
    {
        float fallDelay = fallIndex * delayBetweenPieces;
        Invoke("PreFallShake", fallDelay);
    }

    void PreFallShake()
    {
        Debug.Log("Prefallshake invoked for " + gameObject.name);
        StartCoroutine(ShakeAndFall());
    }

    IEnumerator ShakeAndFall()
    {
        Debug.Log("Shaking " + gameObject.name);
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
        Debug.Log("Falling " + gameObject.name + " before: " + rb.bodyType);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Debug.Log("Falling " + gameObject.name + " after: " + rb.bodyType);
        Debug.Log("Gravity: " + rb.gravityScale);

        Destroy(gameObject, destroyAfter);
    }
}