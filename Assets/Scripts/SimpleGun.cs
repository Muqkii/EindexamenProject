using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    [Header("References")]
    public Camera cam;                 // main camera
    public Transform muzzle;           // where bullets spawn
    public Rigidbody bulletPrefab;     // simple rigidbody projectile

    [Header("Shooting")]
    public bool automatic = false;     // hold to fire if true, click if false
    public float fireRate = 8f;        // rounds per second
    public float bulletSpeed = 60f;    // m/s

    [Header("Ammo")]
    public int magazineSize = 30;
    public int ammoInMag = 30;
    public int reserveAmmo = 90;

    [Header("Aiming")]
    public Vector3 restOffset = new Vector3(0.2f, -0.2f, 0.4f); // local offset from camera
    public Vector3 aimOffset = new Vector3(0.0f, -0.1f, 0.25f);
    public float aimSmooth = 0.08f;
    public float fovNormal = 60f;
    public float fovAim = 40f;

    [Header("Recoil (simple)")]
    public float kickZ = 0.06f;        // backwards kick on shoot
    public float recoilReturn = 0.08f; // how fast it returns

    // --- internals ---
    float cooldown;                    // time until next shot allowed
    Vector3 offsetVel;                 // smoothdamp helper
    float fovVel;                      // smoothdamp helper
    float currentKickZ;                // current recoil position

    void Reset()
    {
        // simple auto-wiring for convenience
        if (!cam) cam = Camera.main;
        if (!muzzle) muzzle = transform;
        ammoInMag = Mathf.Clamp(ammoInMag, 0, magazineSize);
    }

    void Update()
    {
        if (!cam) return;

        HandleAiming();
        HandleShooting();
        HandleReload();

        // keep gun stuck to camera with simple rotation
        transform.rotation = cam.transform.rotation;

        // position gun using camera space offset + recoil
        Vector3 targetOffset = GetTargetOffset() + new Vector3(0, 0, -currentKickZ);
        Vector3 worldPos = cam.transform.position + cam.transform.TransformVector(targetOffset);
        transform.position = worldPos;

        // decay recoil
        currentKickZ = Mathf.SmoothDamp(currentKickZ, 0f, ref fovVel, recoilReturn);
        cooldown -= Time.deltaTime;
    }

    Vector3 GetTargetOffset()
    {
        bool aiming = Input.GetButton("Fire2");
        Vector3 desired = aiming ? aimOffset : restOffset;

        // smooth between rest and aim positions
        Vector3 current = transform.InverseTransformDirection(transform.position - cam.transform.position);
        // we don't actually need current; SmoothDamp directly on an offset we store:
        // keep a persistent smoothed offset instead for stability
        smoothedOffset = Vector3.SmoothDamp(smoothedOffset, desired, ref offsetVel, aimSmooth);

        // also smooth FOV
        float targetFov = aiming ? fovAim : fovNormal;
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref fovVel, aimSmooth);

        return smoothedOffset;
    }

    Vector3 smoothedOffset;

    void HandleAiming()
    {
        // initialize smoothed offset on first frame
        if (Time.frameCount == 1 && smoothedOffset == Vector3.zero)
            smoothedOffset = restOffset;
    }

    void HandleShooting()
    {
        if (cooldown > 0f) return;

        bool wantsToShoot = automatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
        if (!wantsToShoot) return;

        if (ammoInMag <= 0)
            return; // empty – press R to reload

        // spawn projectile
        if (bulletPrefab && muzzle)
        {
            Rigidbody rb = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            rb.velocity = muzzle.forward * bulletSpeed;
        }

        ammoInMag--;
        cooldown = 1f / Mathf.Max(0.01f, fireRate);

        // simple recoil kick (only visual)
        currentKickZ += kickZ;
    }

    void HandleReload()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        if (ammoInMag >= magazineSize) return;
        if (reserveAmmo <= 0) return;

        int needed = magazineSize - ammoInMag;
        int toLoad = Mathf.Min(needed, reserveAmmo);
        ammoInMag += toLoad;
        reserveAmmo -= toLoad;
    }
}
