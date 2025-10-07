
using UnityEngine;
using System.Collections;
using TMPro;

public class GunShoot : MonoBehaviour
{
    public LayerMask layerMask;
    public float damage= 10f;
    public float range = 100f;
    public float fireRate = 15f;
    private int currentAmmo= 15;
    public int maxAmmo = 15;
    public int ammoReserve = 115;
    public float reloadTime= 1f;
    private bool isReloading= false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public Animator animator;

    private float nextTimeToFire = 0f;

    public TextMeshProUGUI ammoText;
    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
      

        if (isReloading)
            return;
        ammoText.text = currentAmmo + " / " + ammoReserve;
        RaycastHit hit;
        if( currentAmmo <=0)
        {
            StartCoroutine(reload());
            return;
        }
        if (Input.GetKey(KeyCode.R)) 
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
        if (ammoReserve <= 0 || currentAmmo == maxAmmo)
        {
            yield break; // stopt de coroutine meteen
        }

        isReloading = true;
        Debug.Log("reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime-.25f);

        

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        // Hoeveel kogels ontbreken?
        int ammoNeeded = maxAmmo - currentAmmo;

        // Check hoeveel er nog in de reserve zit
        int ammoToReload = Mathf.Min(ammoNeeded, ammoReserve);

        // Trek dat van de reserve af
        ammoReserve -= ammoToReload;

        // Vul het magazijn aan
        currentAmmo += ammoToReload;

        //currentAmmo = maxAmmo;
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
