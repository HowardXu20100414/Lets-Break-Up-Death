using UnityEngine;

public class CarHitPlayer : MonoBehaviour
{
    // Left + up vector, adjust to tune flight arc
    public Vector2 knockbackDirection = new Vector2(-2f, 0.5f);

    // How strong the knockback force is
    public float forceOverride = 25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerKnockback knockback = collision.collider.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                // Override player's knockback force for consistent impact strength
                knockback.knockbackForce = forceOverride;
                knockback.Knockback(knockbackDirection);
            }

            // Optional camera zoom/shake on hit
            if (CameraZoomShake.instance != null)
                CameraZoomShake.instance.TriggerZoomShake();
        }
    }
}
