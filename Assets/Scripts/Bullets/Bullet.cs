using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject destroyedBulletPS;
    [SerializeField] private AudioClip defaultHitAudioClip;
    private Transform player;
    private static float maxDistance = 20;

    void Start(){
        if(!player)
            player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
    void Update(){
        if(Vector2.Distance(transform.position, player.position) > Bullet.maxDistance) DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D c){
        DestroyBullet();
    }

    public virtual void DestroyBullet(){
        if(destroyedBulletPS){
            GameObject particle = Instantiate<GameObject>(destroyedBulletPS, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
            particle.GetComponent<AudioSource>().PlayOneShot(defaultHitAudioClip, 0.02f);
        }
        Destroy(this.gameObject);
    }

}
