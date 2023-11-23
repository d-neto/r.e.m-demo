using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BasicGun {

    public int bulletCount = 3;

    [Range(0, 360)]
    public float maxSpreadAngle = 17f;

    public override void Shoot(){

        if(dustParticle)
            Instantiate<GameObject>(dustParticle, bulletTarget.position, this.transform.rotation);


        Vector3 moveDirection = (this.BulletToDirection - (Vector2) bulletTarget.position);
        
        if(Vector2.Distance(moveDirection + bulletTarget.position, bulletTarget.position) < 0.5f)
            moveDirection = (this.BulletToDirection - (Vector2) transform.position);

        moveDirection.z = 0;
        moveDirection.Normalize();

        for (int i = 0; i < bulletCount; i++)
        {
            float normalizedSpread = (i / (float)(bulletCount - 1)) * 2f - 1f;
            float spreadAngle = maxSpreadAngle * normalizedSpread;
            Quaternion rotation = Quaternion.Euler(0, 0, spreadAngle);
            Vector2 newDirection = rotation * moveDirection;
            GameObject cloneBullet = Instantiate<GameObject>(bulletPrefab, bulletTarget.position, rotation);
            cloneBullet.GetComponent<Bullet>().SetDamage(bulletDamage);
            cloneBullet.GetComponent<Bullet>().SetDirection(newDirection.normalized);
            cloneBullet.GetComponent<Rigidbody2D>().AddForce(newDirection.normalized * bulletSpeed, ForceMode2D.Impulse);
        }

        timingRate = fireRate;
        loadedAmmo -= 1;
        MyCamera.Instance.ShakeCam(0.3f, 2f, 1.2f);
        this.Audio.PlayOneShot(shootAudioClip, 0.1f);
    }

}