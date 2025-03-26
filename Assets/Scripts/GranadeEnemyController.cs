using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeEnemyController : MonoBehaviour
{
    [Header("GrenadeConfiguración")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 8f;
    public float detectionRange = 5f;
    public float explosionRange = 1.5f;
    public LayerMask playerLayer;
    public int dañoJugador = 4;
    public float explosionDelay = 0.7f;

    [Header("GrenadeHealth")]
    public int grenadeHealth = 1;
    public LayerMask weaponLayer;

    [Header("GrenadePatrol")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private PlayerController playerController;
    private bool chasing = false;
    private bool isExploding = false;
    Animator anim;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        DetectPlayer();
        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    void DetectPlayer()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (detectedPlayer != null)
        {
            playerController = detectedPlayer.GetComponent<PlayerController>();
            chasing = (playerController != null);
        }
    }
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        FlipSprite(targetPoint.position.x);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    void FlipSprite(float targetX)
    {
        if (targetX < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
    void ChasePlayer()
    {
        if (playerController == null || playerController.projectileTarget == null) return;

        transform.position = Vector2.MoveTowards(transform.position, playerController.transform.position, chaseSpeed * Time.deltaTime);

        FlipSprite(playerController.projectileTarget.position.x);

        if (Vector2.Distance(transform.position, playerController.projectileTarget.position) <= explosionRange && !isExploding)
        {
            Explode();
        }
    }
    void Explode()
    {
        isExploding = true;

        anim.SetTrigger("Explode");

        StartCoroutine(WaitAnimation());

        if (playerController != null)
        {
            playerController.TakeDamage(dañoJugador);
        }

        Debug.Log("El jugador ha perdido 2 corazones por la explosión del enemigo.");

    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        Destroy(gameObject);

    }

    private void WaitForSeconds(float v)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int amount)
    {
        grenadeHealth -= amount;
        //Debug.Log("Enemigo recibió " + amount + " de daño. Vida restante: " + grenadeHealth);

        if (grenadeHealth <= 0)
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & weaponLayer) != 0)
        {
            TakeDamage(1);
            //Debug.Log("El enemigo ha recibido daño");
        }
    }
    void Die()
    {
        //Debug.Log("El enemigo ha sido destruido.");
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
