using UnityEngine;
using UnityEngine.SceneManagement;

public class WallTouchRespawn : MonoBehaviour
{
    public float suctionForce = 20f;
    public float suctionDuration = 0.5f;
    public float suctionInterval = 3f;

    [Tooltip("Assign the wall's particle system here")]
    public ParticleSystem suctionParticles;

    private Rigidbody2D rb;
    private bool isSucking = false;
    private Coroutine suctionCoroutine;
    private Transform currentWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (suctionParticles == null)
        {
            Debug.LogWarning("Suction particles not assigned in inspector!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SuctionWall") && !isSucking)
        {
            currentWall = other.transform;
            suctionCoroutine = StartCoroutine(SuctionLoop());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SuctionWall") && isSucking)
        {
            StopCoroutine(suctionCoroutine);
            isSucking = false;
            currentWall = null;
        }
    }

    System.Collections.IEnumerator SuctionLoop()
    {
        isSucking = true;

        while (true)
        {
            Debug.Log("Suction started!");

            if (suctionParticles != null)
            {
                suctionParticles.Play();
            }

            float elapsed = 0f;
            rb.velocity = Vector2.zero;

            while (elapsed < suctionDuration)
            {
                Vector2 dir = (currentWall.position - transform.position).normalized;
                rb.AddForce(dir * suctionForce * Time.deltaTime, ForceMode2D.Impulse);
                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(suctionInterval);
        }
    }
}
