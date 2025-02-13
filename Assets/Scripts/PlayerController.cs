using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerConfiguraci�n")]
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int health = 10;
    public int maxHealth = 10;
    public int minHealth = 0;
    public int currentHealth;
    public bool isDeath = false;

    [Header("GroundConfiguraci�n")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
