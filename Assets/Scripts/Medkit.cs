using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                playerStats.health = healthAdder(playerStats.health);
                Destroy(gameObject);
            }
        }
    }

    int healthAdder(int health)
    {
        health += healthGain;
        if (health > 100)
        {
            health = 100;
        }
        return (health);
    }
}
