using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinaController : MonoBehaviour
{
    [Header("EnemyFallConfiguración")]
    public float detectionRange = 3f;
    public float fallSpeed = 10f;
    public float returnSpeed = 3f;
    public float fallDelay = 0.5f;
    public Transform startPosition;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    [Header("Sonido de Golpe")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    private bool isFalling = false;
    private bool isReturning = false;
    private Rigidbody2D rb;
    private Animator animPina;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animPina = GetComponent<Animator>();

        rb.isKinematic = true;
        animPina.SetBool("isFalling", false);
    }
    void Update()
    {
        DetectPlayer();
    }
    void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (!isFalling && !isReturning && playerCollider != null)
        {
            Transform player = playerCollider.transform;

            if (Mathf.Abs(player.position.x - transform.position.x) <= 0.5f && player.position.y < transform.position.y)
            {
                StartFall();
            }
        }
    }
    IEnumerator FallDelayCoroutine()
    {
        yield return new WaitForSeconds(fallDelay);
        StartFall();
    }
    void StartFall()
    {
        isFalling = true;
        rb.isKinematic = false;
        rb.velocity = new Vector2(0, -fallSpeed);
        animPina.SetBool("isFalling", true);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(10);
            }

            PlayHitSound();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Invoke(nameof(ReturnToStart), 1f);

            PlayHitSound();
        }
    }
    void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
    void ReturnToStart()
    {
        animPina.SetBool("isFalling", false);
        isReturning = true;
        StartCoroutine(ReturnCoroutine());
    }
    IEnumerator ReturnCoroutine()
    {
        while (Vector3.Distance(transform.position, startPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, returnSpeed * Time.deltaTime);
            yield return null;
        }
        isReturning = false;
        isFalling = false;
        //animPina.SetBool("isFalling", false);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
