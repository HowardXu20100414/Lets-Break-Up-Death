using UnityEngine;

public class RandomParabolaFlyWithSpin : MonoBehaviour
{
    public Vector2 launchVelocityXRange = new Vector2(-8f, -3f);  // Negative range for flying left
    public Vector2 launchVelocityYRange = new Vector2(5f, 12f);   // Vertical speed stays positive
    public Vector2 spinSpeedRange = new Vector2(-720f, 720f);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float randomX = Random.Range(launchVelocityXRange.x, launchVelocityXRange.y);
        float randomY = Random.Range(launchVelocityYRange.x, launchVelocityYRange.y);
        float randomSpin = Random.Range(spinSpeedRange.x, spinSpeedRange.y);

        Vector2 launchVelocity = new Vector2(randomX, randomY);
        rb.velocity = launchVelocity;
        rb.angularVelocity = randomSpin;
    }
}
