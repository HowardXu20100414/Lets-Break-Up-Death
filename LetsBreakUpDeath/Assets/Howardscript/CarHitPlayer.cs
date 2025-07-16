using UnityEngine;

public class CarHitPlayer : MonoBehaviour
{
    public Vector2 knockbackDirection = new Vector2(-2f, 0.5f);
    public float knockbackForceMagnitude = 25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                Vector2 force = knockbackDirection.normalized * knockbackForceMagnitude;
                player.ApplyKnockback(force);
            }
        }
    }
}
