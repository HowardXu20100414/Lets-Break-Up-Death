using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject portal; // This should be assigned to the specific portal the player is entering

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public LayerMask floorLayer;
    float groundCheckRadius = .2f;

    [Header("Jump")]
    int maxJumpsInAir = 1; // 1 = double jump
    int jumpsRemaining = 0;

    [Header("Dash")]
    float dashForce = 25f;
    float dashCooldownMax = 1.5f;
    float dashCooldown;

    float dashDuration = 0.25f; // how long dash lasts
    bool isDashing = false;
    float dashTimeRemaining;

    // --- NEW: Floating State Variables ---
    private bool isFloatingInPortal = false;
    private float originalGravityScale; // To store the player's original gravity scale

    // --- NEW: Bobbing Parameters ---
    [Header("Portal Bobbing")]
    private float bobHeight = 0.2f; // How much the player bobs up and down
    private float bobSpeed = 5f;    // How fast the player bobs
    private float moveIntoPortalDuration = 0.5f; // How long it takes to move to portal center

    private Vector3 portalCenterPosition; // The calculated center position inside the portal

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // Store original gravity scale

        // If the portal is not assigned in the Inspector, try to find it by tag or name
        // It's best to assign it manually or have the Portal script pass its position.
        // For simplicity, we'll assume it's assigned or found.
        if (portal == null)
        {
            portal = GameObject.FindGameObjectWithTag("Portal"); // Example: if your portal has a "Portal" tag
            if (portal == null)
            {
                Debug.LogWarning("Portal GameObject not assigned in PlayerController and not found by tag 'Portal'. Bobbing may not work as expected.", this);
            }
        }
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, floorLayer);
    }

    void Update()
    {
        // --- NEW: Handle bobbing if floating ---
        if (isFloatingInPortal)
        {
            // Apply bobbing motion using Mathf.Sin
            float bobbingOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            // The player's X position remains fixed at the portal's center X
            // The player's Y position bobs around the portal's center Y
            rb.position = new Vector2(portalCenterPosition.x, portalCenterPosition.y + bobbingOffset);

            rb.velocity = Vector2.zero; // Ensure no lingering velocity from physics
            return; // Skip the rest of the Update loop
        }

        dashCooldown -= Time.deltaTime;

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;

            int facingDirection = transform.localScale.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(facingDirection * dashForce, 1.5f);

            if (dashTimeRemaining <= 0)
            {
                isDashing = false;
            }

            return; // skip normal input while dashing
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // flip sprite based on input
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        float nextVelocityX = horizontalInput * moveSpeed;
        float nextVelocityY = rb.velocity.y;

        // Reset jumps when grounded
        if (CheckGrounded() && nextVelocityY <= 0)
        {
            jumpsRemaining = maxJumpsInAir;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpsRemaining > 0)
        {
            if (!CheckGrounded())
            {
                jumpsRemaining -= 1;
            }
            nextVelocityY = jumpSpeed;
        }

        // dash input
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0 && !CheckGrounded())
        {
            //Debug.Log("Dash activated!");

            isDashing = true;
            dashTimeRemaining = dashDuration;
            dashCooldown = dashCooldownMax;

            return; // skip normal input this frame too
        }

        if (CheckGrounded())
        {
            dashCooldown = 0;
        }

        rb.velocity = new Vector2(nextVelocityX, nextVelocityY);
    }

    // --- NEW: Public method to control floating state with bobbing ---
    public void SetFloatingInPortal(bool floating, Vector3 portalPos)
    {
        isFloatingInPortal = floating;
        if (floating)
        {
            portalCenterPosition = portalPos; // Store the portal's center
            rb.gravityScale = 0f;               // Disable gravity
            rb.velocity = Vector2.zero;         // Stop current velocity

            StopAllCoroutines(); // Stop any previous bobbing coroutines if active
            StartCoroutine(MoveToPortalAndBob()); // Start the new coroutine
        }
        else
        {
            rb.gravityScale = originalGravityScale; // Restore original gravity
            StopAllCoroutines(); // Stop bobbing when no longer floating
        }
    }

    // Coroutine to move player to portal center and then start bobbing
    IEnumerator MoveToPortalAndBob()
    {
        Vector3 startPosition = transform.position;
        float timer = 0f;

        // Phase 1: Move to portal center smoothly
        while (timer < moveIntoPortalDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / moveIntoPortalDuration;
            transform.position = Vector3.Lerp(startPosition, portalCenterPosition, progress);
            yield return null;
        }
        transform.position = portalCenterPosition; // Ensure exact final position

        // Phase 2: Bobbing (handled continuously in Update while isFloatingInPortal is true)
        // This coroutine finishes here, and the Update method takes over for the continuous bobbing.
    }
}