using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   // if you use TextMeshPro

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text healthText;       // drag the HealthText UI object here

    private PlayerStats stats;

    private void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Update Health
        //healthText.text = "Health: " + playerStats.health.ToString();
        healthText.text = "Health " + stats.health.ToString();
    }
}
