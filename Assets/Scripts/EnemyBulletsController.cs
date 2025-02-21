using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float detectionRange = 5f;
    public LayerMask playerLayer;
    public float projectileSpeed = 5f;

    private float nextFireTime;

    void Update()
    {
        DetectAndShoot();
    }
    void DetectAndShoot()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (player != null && Time.time >= nextFireTime)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null && playerController.projectileTarget != null)
            {
                Shoot(playerController.projectileTarget.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProyectilController projectileScript = projectile.GetComponent<ProyectilController>();
        if (projectileScript != null)
        {
            projectileScript.SetShooter(this);
            projectileScript.SetTarget(targetPosition, projectileSpeed);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
