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

    [Header("CoinConfiguración")]
    int points = 0;
    public TextMeshProUGUI pointsText;

    [Header("PlayerHealth")]
    public int maxHealth = 4;
    public int currentHealth;
    public GameObject[] lifeBar;

    [Header("GroundConfiguración")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("SoundsConfiguración")]
    public AudioClip healSound;
    public AudioClip jumpSound;
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip coinSound;
    AudioSource audioSourcePlayer;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSourcePlayer = GetComponent<AudioSource>();
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

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSourcePlayer.PlayOneShot(jumpSound);
            anim.SetBool("isJumping", true);

        }
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            audioSourcePlayer.PlayOneShot(attackSound);
        }
    }
    public void AddPoints(int coin)
    {
        points += coin;

        audioSourcePlayer.PlayOneShot(coinSound);
        pointsText.text = points.ToString();
        CheckWinCondition();
    }
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDeath = true;
            Die();
        }
        UpdateLifeBar();
    }
    public bool AddHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            audioSourcePlayer.PlayOneShot(healSound);
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            UpdateLifeBar();
            return true;
        }
        return false;
    }
    void UpdateLifeBar()
    {
        for (int i = 0; i < lifeBar.Length; i++)
        {
            lifeBar[i].SetActive(i < currentHealth);
        }
    }
    private void CheckWinCondition()
    {
        if (points == 5)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
    void Die()
    {
        anim.SetTrigger("Death");
        isDeath = true;
        Debug.Log("El jugador ha muerto. Cargando escena en 2 segundos...");
        enabled = false;
        StartCoroutine(DieAfterDelay(1.2f));
    }

    IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOverScene");
    }
}
