using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController controller; // Replace with your player movement script name

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>(); // Replace if needed
    }

    public void Freeze()
    {
        if (controller != null) controller.enabled = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void Unfreeze()
    {
        if (controller != null) controller.enabled = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
