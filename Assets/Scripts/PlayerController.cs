using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerConfiguración")]
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isDeath = false;
    Animator anim;
    public Transform projectileTarget;

    [Header("PlayerHealth")]
    public int maxHealth = 4;
    public int currentHealth;
    public GameObject[] lifeBar;

    [Header("GroundConfiguración")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        anim.SetBool("isWalking", moveInput != 0);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDeath = true;
            Die();
        }
        UpdateLifeBar();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateLifeBar();
    }
    void UpdateLifeBar()
    {
        for (int i = 0; i < lifeBar.Length; i++)
        {
            lifeBar[i].SetActive(i < currentHealth);
        }
    }
    void Die()
    {
        isDeath = true;
        Debug.Log("El jugador ha muerto. Cargando escena en 2 segundos...");
        enabled = false;
        StartCoroutine(DieAfterDelay(2f));
    }

    IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOverScene");
    }
}
