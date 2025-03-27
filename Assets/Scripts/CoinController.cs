using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int value = 1;
    public LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            collision.GetComponent<PlayerController>().AddPoints(value);
            Destroy(gameObject);
        }
    }
}
