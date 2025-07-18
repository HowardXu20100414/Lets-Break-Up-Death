using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveRightFollowPlayer : MonoBehaviour
{
    public Transform player;       // Assign the player object in the Inspector
    public float baseSpeed = 5f;   // Normal movement speed
    public float maxDistance = 10f; // Distance after which speed increases
    public float speedBoost = 10f;  // Additional speed when far from player

    void Update()
    {
        if (player == null) return;

        float distance = player.position.x - transform.position.x;
        float currentSpeed = baseSpeed;

        // Increase speed if the object is too far behind
        if (distance > maxDistance)
        {
            currentSpeed += speedBoost;
        }

        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}

