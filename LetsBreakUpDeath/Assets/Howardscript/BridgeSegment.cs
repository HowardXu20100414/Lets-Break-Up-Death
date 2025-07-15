using System.Linq;
using UnityEngine;

public class BridgeSegment : MonoBehaviour
{
    public float delayBetweenPieces = 2f;
    [HideInInspector]
    public int fallIndex = 0;
    public float destroyAfter = 5f;
    float fallDelay;
    public int bridgeSegmentNumber;
    public float firstSegmentDelay = 5f; //delay before first bridge segment falls
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Start()
    {
        AssignFallIndex();

        if (fallIndex == 0)
        {
            fallDelay = firstSegmentDelay;
        }
        else
        {
            fallDelay = fallIndex * delayBetweenPieces + firstSegmentDelay;
        }

        if (fallIndex % 2 == 1)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        Invoke("Fall", fallDelay);
    }

    void AssignFallIndex()
    {
        // Find all BridgeFallingPiece in the scene 
        BridgeSegment[] allPieces = null;

        if (transform.parent != null)
        {
            // If the pieces are siblings under the same parent
            allPieces = transform.parent.GetComponentsInChildren<BridgeSegment>();
        }
        else
        {
            // Otherwise find all in scene
            allPieces = FindObjectsOfType<BridgeSegment>();
        }

        // Sort by x position
        var sorted = allPieces.OrderBy(p => p.transform.position.x).ToList();

        // Assign index based on sorted order
        for (int i = 0; i < sorted.Count; i++)
        {
            if (sorted[i] == this)
            {
                fallIndex = i;
                break;
            }
        }
    }

    public void Fall()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic) return;

        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyAfter);
    }
}