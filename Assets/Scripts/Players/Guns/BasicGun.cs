using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : Gun {
    
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject dustParticle;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletDamage;
    protected bool canShoot = true;

    protected float timingRate = 0f;
    protected bool isReloading;
    protected bool isInfinite = true;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnableGun(){
        this.isReloading = false;
        if(this.transform.childCount <= 0)
            this.SetupGun(withPlayer.Data.GetWeapon(referenceCode), withPlayer.GetAIM());
    }

    public override void ShootingController(){
        
        if(timingRate > 0)
            timingRate -= Time.deltaTime;

        if(withPlayer.GetInput().GetReload() && !isReloading) Reload();
        if(IsFireShootPressed()){
            if(timingRate <= 0 && loadedAmmo > 0)
                Shoot();
            else if(loadedAmmo <= 0 && (currentAmmo > 0 || isInfinite))
                Reload();
        }
        if(withPlayer.GetInput().GetFireDown() && loadedAmmo <= 0 && currentAmmo <= 0 && timingRate <= 0 && !isInfinite){
            timingRate = fireRate;
            Audio.PlayOneShot(emptyAudioClip, 0.1f);
        }
    }

    public override void Shoot(){

        if(dustParticle)
            Instantiate<GameObject>(dustParticle, bulletTarget.position, this.transform.rotation);

        GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, this.transform.rotation);
        cloneBullet.GetComponent<Bullet>().Setup(withPlayer.transform, bulletDamage);

        Vector3 moveDirection = (this.BulletToDirection - (Vector2) bulletTarget.position);
        
        if(Vector2.Distance(moveDirection + bulletTarget.position, bulletTarget.position) < 0.5f)
            moveDirection = (this.BulletToDirection - (Vector2) transform.position);

        moveDirection.z = 0;       
        moveDirection.Normalize();
        cloneBullet.GetComponent<Bullet>().SetDirection(moveDirection);

        cloneBullet.GetComponent<Rigidbody2D>().AddForce(moveDirection * bulletSpeed, ForceMode2D.Impulse);
        timingRate = fireRate;
        loadedAmmo -= 1;
        MyCamera.Instance.ShakeCam(0.2f, 1.4f, 1f);
        this.Audio.PlayOneShot(shootAudioClip, 0.1f);
    }

    public override void Reload(){
        this.Audio.PlayOneShot(reloadAudioClip, 0.1f);
        if(!isReloading){
            StartCoroutine(ReloadingAmmo());
        }
        isReloading = true;
        withPlayer.GetInput().Reloading(this, isReloading);
    }

    IEnumerator ReloadingAmmo(){
        if(currentAmmo - maxAmmo >= 0 || isInfinite){
            currentAmmo -= (maxAmmo-loadedAmmo);
            yield return new WaitForSeconds(reloadTime);
            loadedAmmo = maxAmmo;
        }else{
            yield return new WaitForSeconds(reloadTime);
            loadedAmmo = currentAmmo;
            currentAmmo = 0;
        }

        isReloading = false;
        withPlayer.GetInput().Reloading(this, isReloading);
    }

    bool IsFireShootPressed(){
        return (withPlayer.GetInput().GetFire() || (withPlayer.GetAIM().mode == AIM_MODE.JoystickAuto && withPlayer.GetInput().GetAxisAnalogMaxOffset(0.3f))) && !isReloading && canShoot;
    }

}