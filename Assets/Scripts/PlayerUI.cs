using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   // if you use TextMeshPro

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;   // drag your Player object here
    public GunShoot gun;              // drag your Gun object here

    [Header("UI Elements")]
    public TMP_Text healthText;       // drag the HealthText UI object here
    public TMP_Text ammoText;         // drag the AmmoText UI object here

    void Update()
    {
        // Update Health
        healthText.text = "Health: " + playerStats.health.ToString();

        // Update Ammo
        ammoText.text = "" + gun.currentAmmo + " / " + gun.reserveAmmo;
    }
}
