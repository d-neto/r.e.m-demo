using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Vector3 direction;
    [SerializeField] private float damage;
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

    public float GetDamage() => this.damage;
    public void SetDamage(float damage) => this.damage = damage;
    public Vector3 GetDirection() => this.direction;
    public void SetDirection(Vector3 direction) => this.direction = direction;

    public void Setup(Transform player, float damage){
        SetDamage(damage);
        this.player = player;
    }
    public void Setup(Transform player, Vector3 direction, float damage){
        Setup(player, damage);
        SetDirection(direction);
    }
}
