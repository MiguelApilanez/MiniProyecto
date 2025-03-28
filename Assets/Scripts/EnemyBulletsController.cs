using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsController : MonoBehaviour
{
    [Header("EnemyBulletConfiguración")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.05f;
    public float detectionRange = 8f;
    public LayerMask playerLayer;
    public float projectileSpeed = 5.5f;
    private float nextFireTime;
    private Transform playerTransform;

    [Header("Drop de Moneda")]
    public GameObject coinPrefab;

    [Header("EnemyBulletHealth")]
    public LayerMask weaponLayer;
    public int health = 3;

    [Header("Sonido de Disparo")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    private Animator anim;
    private bool isAttacking = false;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("Attack", false);
    }

    void Update()
    {
        DetectAndShoot();
        LookAtPlayer();
    }
    void DetectAndShoot()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (player != null)
        {
            playerTransform = player.transform;

            if (Time.time >= nextFireTime)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerController != null && playerController.projectileTarget != null)
                {
                    Shoot(playerController.projectileTarget.position);
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
    void LookAtPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            if (direction.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Shoot(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProyectilController projectileScript = projectile.GetComponent<ProyectilController>();
        anim.SetBool("Attack", true);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (projectileScript != null)
        {
            projectileScript.SetShooter(this);
            projectileScript.SetTarget(targetPosition, projectileSpeed);
        }
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemigo recibió " + amount + " de daño. Vida restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & weaponLayer) != 0)
        {
            TakeDamage(1);
            Debug.Log("El enemigo ha recibido daño");
        }
    }
    void Die()
    {
        Debug.Log("El enemigo ha sido destruido.");

        if (coinPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 2f, 0);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
