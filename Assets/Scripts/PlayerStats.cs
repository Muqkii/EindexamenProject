using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;

    private void Update()
    {
        if(health <= 0)
        {
            //death
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}

