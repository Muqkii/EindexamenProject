
using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour
{
    public LayerMask layerMask;
    public float damage= 10f;
    public float range = 100f;
    public float fireRate = 15f;
    private int currentAmmo= 15;
    public int maxAmmo = 15;
    public float reloadTime= 1f;
    private bool isReloading= false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public Animator animator;

    private float nextTimeToFire = 0f;
    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        RaycastHit hit;
        if( currentAmmo <=0)
        {
            StartCoroutine(reload());
            return;
        }

        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire)
        {

           nextTimeToFire = Time.time + 1f/ fireRate;
            Shoot();

        }
        
    }
     
    IEnumerator  reload() 
    {
        isReloading = true;
        Debug.Log("reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime-.25f);

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);



        currentAmmo = maxAmmo;
        isReloading=false;
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            currentAmmo--;
            
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
