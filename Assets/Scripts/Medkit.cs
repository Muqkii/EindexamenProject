using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healthGain = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerStats = other.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Heal(healthGain); // use PlayerStats' Heal method
                Destroy(gameObject);
            }
        }
    }
}
