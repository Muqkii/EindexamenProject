using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 200;   // maximum possible health
    public int health = 100;      // starting health

    void Start()
    {
        // Ensure health never goes above maxHealth
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    private void Update()
    {
        if(health <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
            //death
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Player Health: " + health);
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Player Healed. Current Health: " + health);
    }
}

