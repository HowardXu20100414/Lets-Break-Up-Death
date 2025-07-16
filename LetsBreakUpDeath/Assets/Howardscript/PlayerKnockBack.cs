using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public float knockbackForce = 20f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction)
    {
        rb.velocity = Vector2.zero;

        Debug.Log($"Knockback force vector: {direction * knockbackForce}");
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
