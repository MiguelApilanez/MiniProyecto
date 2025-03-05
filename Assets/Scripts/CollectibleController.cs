using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public int healthAmount = 1;
    public LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                if (player.AddHealth(healthAmount))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
