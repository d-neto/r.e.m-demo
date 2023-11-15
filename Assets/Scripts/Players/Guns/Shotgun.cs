using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BasicGun {

    public int bulletCount = 3;
    public float angleBullet = 0.25f;
    public override void Shoot(){

        if(dustParticle)
            Instantiate<GameObject>(dustParticle, bulletTarget.position, this.transform.rotation);


        Vector3 moveDirection = (this.BulletToDirection - (Vector2) bulletTarget.position);
        
        if(Vector2.Distance(moveDirection + bulletTarget.position, bulletTarget.position) < 0.5f)
            moveDirection = (this.BulletToDirection - (Vector2) transform.position);

        moveDirection.z = 0;       
        moveDirection.Normalize();

        for(int i = 1; i <= bulletCount/2; i++){
            Vector2 newDirection = moveDirection;
            newDirection.y += i*angleBullet;
            newDirection.x += i*angleBullet;
            GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, this.transform.rotation);
            cloneBullet.GetComponent<Bullet>().SetDamage(bulletDamage);
            cloneBullet.GetComponent<Bullet>().SetDirection(newDirection.normalized);
            cloneBullet.GetComponent<Rigidbody2D>().AddForce(newDirection.normalized * bulletSpeed, ForceMode2D.Impulse);
        }
        for(int i = (bulletCount/2)+1; i < bulletCount; i++){
            Vector2 newDirection = moveDirection;
            newDirection.y -= (i-(bulletCount/2))*angleBullet;
            newDirection.x -= (i-(bulletCount/2))*angleBullet;
            GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, this.transform.rotation);
            cloneBullet.GetComponent<Bullet>().SetDamage(bulletDamage);
            cloneBullet.GetComponent<Bullet>().SetDirection(newDirection.normalized);
            cloneBullet.GetComponent<Rigidbody2D>().AddForce(newDirection.normalized * bulletSpeed, ForceMode2D.Impulse);
        }

        if(bulletCount%2 != 0){
            GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, this.transform.rotation);
            cloneBullet.GetComponent<Bullet>().SetDamage(bulletDamage);
            cloneBullet.GetComponent<Bullet>().SetDirection(moveDirection);
            cloneBullet.GetComponent<Rigidbody2D>().AddForce(moveDirection * bulletSpeed, ForceMode2D.Impulse);
        }

        timingRate = fireRate;
        loadedAmmo -= 1;
        this.Audio.PlayOneShot(shootAudioClip, 0.1f);

    }

}