using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : Gun {
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletTarget;
    [SerializeField] private float bulletSpeed;
    float timingRate = 0f;
    bool isReloading;

    public override void ShootingController(){
        
        if(timingRate > 0)
            timingRate -= Time.deltaTime;

        if(Input.GetButton("Fire1") && !isReloading){
            if(timingRate <= 0 && loadedAmmo > 0)
                Shoot();
            else if(loadedAmmo <= 0 && currentAmmo > 0)
                Reload();
        }
        if(Input.GetButtonDown("Fire1") && loadedAmmo <= 0 && currentAmmo <= 0)
            Debug.Log("No AMMO!!");
    }

    public override void Shoot(){
        GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, this.transform.rotation);
        cloneBullet.GetComponent<Rigidbody2D>().AddForce(cloneBullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
        timingRate = fireRate;
        loadedAmmo -= 1;
    }

    public override void Reload(){
        Debug.Log("Reloading");
        if(!isReloading){
            StartCoroutine(ReloadingAmmo());
        }
        isReloading = true;
    }

    IEnumerator ReloadingAmmo(){
        if(currentAmmo - maxAmmo >= 0){
            currentAmmo -= maxAmmo;
            yield return new WaitForSeconds(reloadTime);
            loadedAmmo = maxAmmo;
        }else{
            yield return new WaitForSeconds(reloadTime);
            loadedAmmo = currentAmmo;
            currentAmmo = 0;
        }

        isReloading = false;
    }

}