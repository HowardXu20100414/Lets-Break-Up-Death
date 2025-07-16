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
    public GameObject randomTriggerPrefab;

    public int fallIndex = 0;
    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;


        if (Random.value < .2)
        {
            Instantiate(randomTriggerPrefab, new Vector3(transform.position.x + 3, transform.position.y + 10, transform.position.z), Quaternion.identity);
        }
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
    }

    public void TriggerFallSequence()
    {
        float fallDelay = fallIndex * delayBetweenPieces;
        Invoke("PreFallShake", fallDelay);
    }

    void PreFallShake()
    {
        StartCoroutine(ShakeAndFall());
    }

    public IEnumerator ShakeAndFall()
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