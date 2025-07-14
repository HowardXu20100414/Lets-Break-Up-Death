using UnityEngine;
using System.Linq;

public class BridgeFallingPiece : MonoBehaviour
{
    public float delayBetweenPieces;
    [HideInInspector]
    public int fallIndex = 0;
    public float destroyAfter = 5f;
    public bool firstBridgePiece = false;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Start()
    {
        if (!firstBridgePiece)
        {
            delayBetweenPieces = Random.Range(1.5f, 2.5f);
        } else
        {
            delayBetweenPieces = 5f;
        }

        AssignFallIndex();

        float fallDelay = fallIndex * delayBetweenPieces;
        Invoke(nameof(Fall), fallDelay);
    }

    void AssignFallIndex()
    {
        // Find all BridgeFallingPiece in the scene 
        BridgeFallingPiece[] allPieces = null;

        if (transform.parent != null)
        {
            // If the pieces are siblings under the same parent
            allPieces = transform.parent.GetComponentsInChildren<BridgeFallingPiece>();
        }
        else
        {
            // Otherwise find all in scene (less efficient)
            allPieces = FindObjectsOfType<BridgeFallingPiece>();
        }

        // Sort by x position ascending (left to right)
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
