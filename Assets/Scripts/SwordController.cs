using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackRate = 0.5f;
    public LayerMask enemyLayer;
    public LayerMask bulletLayer;

    private float nextAttackTime = 0f;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyBulletsController enemyController = enemy.GetComponent<EnemyBulletsController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
                Debug.Log("Golpeaste al enemigo");
            }
        }

        Collider2D[] hitBullets = Physics2D.OverlapCircleAll(transform.position, attackRange, bulletLayer);
        foreach (Collider2D bullet in hitBullets)
        {
            Destroy(bullet.gameObject);
            Debug.Log("Destruiste un proyectil enemigo");
        }
    }
}
