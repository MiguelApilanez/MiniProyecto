using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float damageInterval = 1f;
    public LayerMask playerLayer;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(other.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        while (playerController != null && playerController.currentHealth > 0)
        {
            playerController.TakeDamage(1);
            //Debug.Log("El jugador ha perdido 1 corazón por la trampa.");
            yield return new WaitForSeconds(damageInterval);
        }

        damageCoroutine = null;
    }
}
