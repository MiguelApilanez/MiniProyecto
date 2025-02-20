using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilController : MonoBehaviour
{
    private EnemyBulletsController shooter;
    public LayerMask playerLayer;
    private Vector2 targetDirection;
    private float speed;
    public float destroyBullet = 2.5f;

    void Start()
    {
        Destroy(gameObject, destroyBullet);
    }
    public void SetShooter(EnemyBulletsController shooter)
    {
        this.shooter = shooter;
    }

    public void SetTarget(Vector2 targetPosition, float speed)
    {
        this.speed = speed;
        targetDirection = (targetPosition - (Vector2)transform.position).normalized;
    }
    void Update()
    {
        transform.position += (Vector3)(targetDirection * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
