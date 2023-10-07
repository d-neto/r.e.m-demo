using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject destroyedBulletPS;
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
        if(destroyedBulletPS)
            Instantiate(destroyedBulletPS, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
        Destroy(this.gameObject);
    }

}
