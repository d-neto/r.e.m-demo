using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] protected MeleeEnemyData Data;
    [SerializeField] protected Animator Anim;
    [SerializeField] protected Rigidbody2D rb2D;

    [SerializeField] protected Transform target;

    protected bool isDead;

    void Awake(){
        if(!rb2D) rb2D = GetComponent<Rigidbody2D>();
        if(!Anim) Anim = GetComponent<Animator>();
        Anim.runtimeAnimatorController = Data.animator;
        this.Data = Instantiate(this.Data);

        this.OnAwake();
    }

    void Update(){
        if(isDead) return;
        OnUpdate();

        if(Data.life <= 0 && !isDead) OnDeath();
    }

    public virtual void OnUpdate(){
        // Do Something...
    }

    public virtual void OnAwake(){
        // Do Something...
    }


    public virtual void OnDeath(){
        isDead = true;
    }

    public virtual void OnDamage(float damage, Vector3 origin){
        this.Data.life -= damage;
    }

    public MeleeEnemyData GetData() => this.Data;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 11)//Player Bullet
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>() ?? null;
            this.OnDamage(bullet.GetDamage(), bullet.GetDirection());
        }

        if(other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<Player>().Stats.TakeDamage(this.Data.damage, this.transform);
    }
}