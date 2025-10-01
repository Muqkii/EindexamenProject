using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("Gun Settings")]
    public LayerMask layerMask;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    [Header("Ammo Settings")]
    public int maxAmmo = 30;      // total ammo per magazine
    public int currentAmmo;       // current ammo in mag
    public int reserveAmmo = 90;  // extra ammo carried
    public float reloadTime = 2f; // seconds to reload

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo; // fill mag on start
    }

    void Update()
    {
        if (isReloading)
            return;

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Shooting
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // Auto reload when ammo is 0
        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        currentAmmo--; // reduce ammo

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
    }
}
