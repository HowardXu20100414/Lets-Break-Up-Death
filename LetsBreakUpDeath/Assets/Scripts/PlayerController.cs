using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject portal;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public LayerMask floorLayer;
    float groundCheckRadius = .2f;

    [Header("Jump")]
    int maxJumpsInAir = 1;
    int jumpsRemaining = 0;

    [Header("Dash")]
    float dashForce = 25f;
    float dashCooldownMax = 1.5f;
    float dashCooldown;

    float dashDuration = 0.25f;
    bool isDashing = false;
    float dashTimeRemaining;

    private bool isFloatingInPortal = false;
    private float originalGravityScale;

    [Header("Portal Bobbing")]
    private float bobHeight = 0.2f;
    private float bobSpeed = 5f;
    private float moveIntoPortalDuration = 0.5f;
    private Vector3 portalCenterPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;

        if (portal == null)
        {
            portal = GameObject.FindGameObjectWithTag("Portal");
            if (portal == null)
            {
                Debug.LogWarning("Portal GameObject not assigned or found.");
            }
        }
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, floorLayer);
    }

    void Update()
    {
        dashCooldown -= Time.deltaTime;

        if (isFloatingInPortal)
        {
            float bobbingOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            rb.position = new Vector2(portalCenterPosition.x, portalCenterPosition.y + bobbingOffset);
            rb.velocity = Vector2.zero;
            return;
        }

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;

            int facingDirection = transform.localScale.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(facingDirection * dashForce, 1.5f);

            if (dashTimeRemaining <= 0)
            {
                isDashing = false;
            }
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else if (horizontalInput > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        float nextVelocityX = horizontalInput * moveSpeed;
        float nextVelocityY = rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpsRemaining > 0)
        {
            if (!CheckGrounded())
                jumpsRemaining--;
            nextVelocityY = jumpSpeed;
        }

        if (CheckGrounded() && nextVelocityY <= 0)
            jumpsRemaining = maxJumpsInAir;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0 && !CheckGrounded())
        {
            isDashing = true;
            dashTimeRemaining = dashDuration;
            dashCooldown = dashCooldownMax;
            return;
        }

        if (CheckGrounded())
            dashCooldown = 0;

        rb.velocity = new Vector2(nextVelocityX, nextVelocityY);
    }

    public void SetFloatingInPortal(bool floating, Vector3 portalPos)
    {
        isFloatingInPortal = floating;
        if (floating)
        {
            portalCenterPosition = portalPos;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(MoveToPortalAndBob());
        }
        else
        {
            rb.gravityScale = originalGravityScale;
            StopAllCoroutines();
        }
    }

    IEnumerator MoveToPortalAndBob()
    {
        Vector3 startPosition = transform.position;
        float timer = 0f;

        while (timer < moveIntoPortalDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / moveIntoPortalDuration;
            transform.position = Vector3.Lerp(startPosition, portalCenterPosition, progress);
            yield return null;
        }
        transform.position = portalCenterPosition;
    }

    // NEW: Just a simple method to add knockback force instantly
    public void ApplyKnockback(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
