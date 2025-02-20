using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeEnemyController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 8f;
    public float detectionRange = 5f;
    public float explosionRange = 1.5f;
    public int explosionDamage = 50;
    public LayerMask playerLayer;
    public Transform[] patrolPoints;
    public int dañoJugador = 4;

    private int currentPatrolIndex = 0;
    private Transform player;
    private bool chasing = false;

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
            player = detectedPlayer.transform;
            chasing = true;
        }
    }
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    void ChasePlayer()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) <= explosionRange)
        {
            Explode();
        }
    }
    void Explode()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, explosionRange, playerLayer);
        if (detectedPlayer != null)
        {
            PlayerController playerController = detectedPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(dañoJugador);
            }
        }

        Debug.Log("El jugador ha perdido 2 corazones por la explosión del enemigo.");
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
