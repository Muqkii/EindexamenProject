using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10; // how much damage enemy deals

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    // If you want trigger-based detection (e.g., invisible hitbox), use this instead:
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
