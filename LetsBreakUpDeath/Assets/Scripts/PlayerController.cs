using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;

    public Transform groundCheckPoint;
    public LayerMask floorLayer;
    float groundCheckRadius = .2f;
    public int maxJumps = 2;
    public int jumpsRemaining = 0;
    float dashCooldown;
    float dashCooldownMax = 5f; //what dash cooldown is set to
    public float dashForce = 30f;


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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
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
        if (CheckGrounded() && nextVelocityY <= 0)
        {
            jumpsRemaining = maxJumps;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpsRemaining > 0) // JUMP BUTTON
        {
            jumpsRemaining -= 1;
            nextVelocityY = jumpSpeed;
        }


        dashCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0)
        {
            Dash(horizontalInput);
            dashCooldown = dashCooldownMax;
        }

        rb.velocity = new Vector2(nextVelocityX, nextVelocityY);
    }


    public void Dash(float horizontalInput)
    {
        Debug.Log("activated dash");
        if (horizontalInput == 0)
        {
            rb.AddForce(Vector2.up * dashForce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(horizontalInput * dashForce, 0), ForceMode2D.Impulse);
        }
    }

}
