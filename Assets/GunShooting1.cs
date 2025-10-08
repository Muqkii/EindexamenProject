using System.Collections;
using UnityEngine;
using TMPro;

public class GunShooting1 : MonoBehaviour
{
    [Header("References")]
    public Camera cam;                     // FPS camera used for raycast + FOV
    public Transform muzzle;               // gun model anchor
    public ParticleSystem muzzleFlash;     // optional
    public GameObject impactEffect;        // optional

    [Header("Fire Settings")]
    public float fireRate = 10f;           // rounds per second
    public float range = 100f;
    public float damage = 10f;
    public LayerMask layerMask = ~0;       // what the ray can hit

    [Header("Ammo")]
    public int maxAmmo = 30;               // bullets per magazine
    public int currentAmmo = 30;           // current bullets in mag
    public int reserveAmmo = 90;           // spare bullets
    public float reloadTime = 2f;

    [Header("Aiming (viewmodel placement)")]
    public Vector3 restOffset = new Vector3(0.2f, -0.2f, 0.4f);   // local offset in camera space
    public Vector3 aimOffset = new Vector3(0.0f, -0.1f, 0.25f);  // tighter while aiming
    public float aimSmooth = 0.08f;
    public float fovNormal = 60f;
    public float fovAim = 40f;

    [Header("Recoil (visual only)")]
    public float kickZ = 0.06f;            // backward kick each shot
    public float recoilReturn = 0.08f;     // return speed

    [Header("UI")]
    public TextMeshProUGUI ammoText;

 

    // --- internals ---
    float nextFireTime;
    bool isReloading;

    Vector3 smoothedOffset;
    Vector3 offsetVel;                     // SmoothDamp helper
    float fovVel;                          // SmoothDamp helper for FOV
    float currentKickZ;
    float kickVel;                         // SmoothDamp helper for recoil

    void Reset()
    {
        if (!cam) cam = Camera.main;
        if (!muzzle) muzzle = transform;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
    }

    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!muzzle) muzzle = transform;
        smoothedOffset = restOffset;       // start at hip/rest
        cam.fieldOfView = fovNormal;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (!cam) return;

        if (PauseMenu1.isPaused)
            return;

        // manual reload
        if (!isReloading && Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        HandleAiming();
        HandleShootInput();

        // simple viewmodel follow (rotation from camera, position from camera + offset + recoil)
        transform.rotation = cam.transform.rotation;

        Vector3 targetOffset = smoothedOffset + new Vector3(0, 0, -currentKickZ);
        Vector3 worldPos = cam.transform.position + cam.transform.TransformVector(targetOffset);
        transform.position = worldPos;

        // decay recoil over time
        currentKickZ = Mathf.SmoothDamp(currentKickZ, 0f, ref kickVel, recoilReturn);
    }

    void HandleAiming()
    {
        bool isAiming = Input.GetButton("Fire2");

        // smooth offset
        Vector3 desired = isAiming ? aimOffset : restOffset;
        smoothedOffset = Vector3.SmoothDamp(smoothedOffset, desired, ref offsetVel, aimSmooth);

        // smooth camera FOV
        float targetFov = isAiming ? fovAim : fovNormal;
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref fovVel, aimSmooth);
    }

    void HandleShootInput()
    {
        if (isReloading) return;

        // auto-reload on empty (if we have reserves)
        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // ALWAYS AUTOMATIC: hold Fire1 to shoot
        bool wantsToShoot = Input.GetButton("Fire1");
        if (!wantsToShoot) return;
        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0) return;

        nextFireTime = Time.time + 1f / Mathf.Max(0.01f, fireRate);
        Shoot();
    }

    void Shoot()
    {
        currentAmmo--;

        // VFX
        if (muzzleFlash) muzzleFlash.Play();

        // raycast from camera for precise hits
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range, layerMask, QueryTriggerInteraction.Ignore))
        {
            // optional: your damage receiver component
            if (hit.transform.TryGetComponent(out Target target))
            {
                target.TakeDamage(damage);
            }

            if (impactEffect)
            {
                var fx = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(fx, 2f);
            }
        }

        // visual recoil kick (viewmodel only)
        currentKickZ += kickZ;
        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int need = maxAmmo - currentAmmo;
        int toLoad = Mathf.Min(need, reserveAmmo);

        currentAmmo += toLoad;
        reserveAmmo -= toLoad;

        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {reserveAmmo}";
    }
}

