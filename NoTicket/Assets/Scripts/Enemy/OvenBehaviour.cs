using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class OvenBehaviour : MonoBehaviour
{
    public float fireRange = 5.0f;
    public float fireDamage = 10.0f;
    public float fireDuration = 2.0f;
    public float fireCooldown = 5.0f;
    public Transform firePoint;
    public LayerMask playerLayer;

    private bool isFiring = false;
    private float nextFireTime = 0.0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Collider2D playerInRange = Physics2D.OverlapCircle(firePoint.position, fireRange, playerLayer);
            if (playerInRange != null)
            {
                StartCoroutine(SpitFire());
                nextFireTime = Time.time + fireCooldown;
            }
        }
    }

    private IEnumerator SpitFire()
    {
        isFiring = true;
        // Trigger fire animation here if any
        yield return new WaitForSeconds(fireDuration);
        isFiring = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, fireRange);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isFiring && other.CompareTag("Player"))
        {
            // Apply damage to the player
            //other.GetComponent<PlayerHealth>().TakeDamage(fireDamage * Time.deltaTime);
        }
    }
}