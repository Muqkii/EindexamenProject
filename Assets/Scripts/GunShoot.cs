
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public LayerMask layerMask;
    public float damage= 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public bool shooting;

    private float nextTimeToFire = 0f;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire)
        {

           nextTimeToFire = Time.time + 1f/ fireRate;
            Shoot();
            Debug.Log("shooting");
            shooting = true;
        }
        else
        {
            shooting = false;
        }
        
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            muzzleFlash.Play();

            GameObject impactGo= Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo,2f);
        }
    }
}
