using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;

public class PinaController : MonoBehaviour
{
    public float detectionRange = 3f;
    public float fallSpeed = 10f;
    public float returnSpeed = 3f;
    public Transform startPosition;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private bool isFalling = false;
    private bool isReturning = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
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
    void StartFall()
    {
        isFalling = true;
        rb.isKinematic = false;
        rb.velocity = new Vector2(0, -fallSpeed);
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
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Invoke(nameof(ReturnToStart), 1f);
        }
    }
    void ReturnToStart()
    {
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
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
