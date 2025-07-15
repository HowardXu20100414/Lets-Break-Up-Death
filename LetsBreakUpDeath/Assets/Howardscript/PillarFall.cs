using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PillarFall : MonoBehaviour
{
    public float delayBeforeFall = 1f;
    public float torqueForce = 200f; // positive for clockwise, negative for counterclockwise

    private Rigidbody2D rb;
    private bool hasFallen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void StartFall()
    {
        if (hasFallen) return;

        hasFallen = true;
        StartCoroutine(FallAfterDelay());
    }

    private IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFall);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddTorque(torqueForce, ForceMode2D.Impulse);
    }
}
