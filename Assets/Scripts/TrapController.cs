using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int damage = 10;
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
        while (true)
        {
            Debug.Log("El jugador ha recibido " + damage + " de daño de la trampa.");
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
