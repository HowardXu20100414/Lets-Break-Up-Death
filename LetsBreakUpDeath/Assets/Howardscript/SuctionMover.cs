using UnityEngine;

public class SuctionMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float suctionRadius = 5f;
    public float suctionForce = 20f;
    public float suctionInterval = 5f;
    public LayerMask playerLayer;
    public WindController windController; // Optional particle controller

    private float suctionTimer;

    void Update()
    {
        // Move the object to the right constantly
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // Countdown for suction burst
        suctionTimer -= Time.deltaTime;

        if (suctionTimer <= 0f)
        {
            SuctionPull();
            suctionTimer = suctionInterval;

            // Trigger particle effect if set
            if (windController != null)
            {
                windController.PlayWindBurst();
            }
        }
    }

    void SuctionPull()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, suctionRadius, playerLayer);

        if (playerCollider != null)
        {
            Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("Suction pulling player...");
                // Only pull to the right (horizontal suction)
                Vector2 directionToRight = new Vector2(1, 0); // Force only along X axis
                playerRb.AddForce(directionToRight * suctionForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, suctionRadius);
    }
}
