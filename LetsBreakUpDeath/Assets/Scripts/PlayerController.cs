using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, floorLayer);
    }

    void Update()
    {
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
            Debug.Log("Dash activated!");

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
}
