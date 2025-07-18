using UnityEngine;
using System.Collections;

public class TunnelFade : MonoBehaviour
{
    public float fadeDuration = 1.5f; // How long it takes for the car to fully disappear
    public float fadeStartDepth = 0.5f; // Normalized depth (0 to 1) within the trigger where fading begins

    private BoxCollider2D tunnelCollider; // Reference to this trigger's collider

    void Start()
    {
        tunnelCollider = GetComponent<BoxCollider2D>();
        if (tunnelCollider == null)
        {
            Debug.LogError("TunnelFade requires a BoxCollider2D on the same GameObject.");
            enabled = false; // Disable script if no collider
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collided object is a car (you might use tags or specific components)
        if (other.CompareTag("Car")) // Make sure your car GameObjects have the tag "Car"
        {
            SpriteRenderer carRenderer = other.GetComponent<SpriteRenderer>();
            if (carRenderer != null)
            {
                // Calculate how deep the car is inside the tunnel trigger, normalized from 0 to 1
                // This assumes the tunnel is horizontal. Adjust for vertical tunnels.
                float carX = other.transform.position.x;
                float colliderMinX = tunnelCollider.bounds.min.x;
                float colliderMaxX = tunnelCollider.bounds.max.x;

                float normalizedDepth = Mathf.InverseLerp(colliderMinX, colliderMaxX, carX);

                // Determine the fade alpha based on depth
                float currentAlpha = 1f;
                if (normalizedDepth > fadeStartDepth)
                {
                    // Calculate fade progress: 0 when fadeStartDepth, 1 when fully inside
                    float fadeProgress = Mathf.InverseLerp(fadeStartDepth, 1f, normalizedDepth);
                    currentAlpha = 1f - fadeProgress; // Fade out from 1 to 0
                }

                // Apply the calculated alpha to the car's sprite color
                Color carColor = carRenderer.color;
                carColor.a = Mathf.Clamp01(currentAlpha); // Clamp between 0 and 1
                carRenderer.color = carColor;

                // If fully transparent, consider deactivating or destroying the car
                if (carColor.a <= 0.01f) // Use a small epsilon to account for float inaccuracies
                {
                    other.gameObject.SetActive(false); // Or Destroy(other.gameObject);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If a car exits the trigger before fully fading, make it visible again
        if (other.CompareTag("Car"))
        {
            SpriteRenderer carRenderer = other.GetComponent<SpriteRenderer>();
            if (carRenderer != null)
            {
                Color carColor = carRenderer.color;
                carColor.a = 1f; // Restore full opacity
                carRenderer.color = carColor;
                other.gameObject.SetActive(true); // Ensure it's active if it was deactivated
            }
        }
    }

    // Optional: Visualize the fade start point in the editor
    void OnDrawGizmos()
    {
        if (tunnelCollider != null)
        {
            Gizmos.color = Color.yellow;
            float fadeStartX = Mathf.Lerp(tunnelCollider.bounds.min.x, tunnelCollider.bounds.max.x, fadeStartDepth);
            Vector3 startPoint = new Vector3(fadeStartX, tunnelCollider.bounds.center.y, 0);
            Vector3 endPoint = new Vector3(fadeStartX, tunnelCollider.bounds.max.y, 0);
            Gizmos.DrawLine(startPoint, new Vector3(fadeStartX, tunnelCollider.bounds.min.y, 0));
            Gizmos.DrawSphere(startPoint, 0.1f);
        }
    }
}