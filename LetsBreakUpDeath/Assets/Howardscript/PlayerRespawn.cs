using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float fallThresholdY = -10f;         // Y position that counts as falling off
    public Transform respawnPoint;              // Assign in Inspector

    void Update()
    {
        if (transform.position.y < fallThresholdY)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;

        // Optional: reset velocity if using Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
