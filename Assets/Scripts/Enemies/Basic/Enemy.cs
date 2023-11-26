using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyData Data;
    [SerializeField] protected Animator Anim;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected Transform target;
    [SerializeField] protected PathFinder Finder;
    protected bool isDead;

    void Awake(){
        if(!rb2D) rb2D = GetComponent<Rigidbody2D>();
        if(!Anim) Anim = GetComponent<Animator>();
        Anim.runtimeAnimatorController = Data.animator;
        this.Data = Instantiate(this.Data);

        this.Finder = new PathFinder(this, GetComponent<Seeker>());

        this.OnAwake();
    }

    void Start(){
        EnemyManager.Instance.RegisterEnemy(this.GetComponent<AbleAim>());
    }

    void Update(){
        if(isDead) Destroy(this.gameObject);
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
        EnemyManager.Instance.UnregisterEnemy(this.GetComponent<AbleAim>());
        isDead = true;
    }

    public virtual void OnDamage(float damage, Vector3 origin){
        this.Data.life -= damage;
    }

    public EnemyData GetData() => this.Data;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 11)//Player Bullet
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>() ?? null;
            this.OnDamage(bullet.GetDamage(), bullet.GetDirection());
        }

        if(other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<Player>().Stats.TakeDamage(this.Data.damage, this.transform);
    }

    public Vector3 GetTargetPosition(){
        GameObject player = null;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            if(player == null || Vector2.Distance(g.transform.position, transform.position) < Vector2.Distance(player.transform.position, transform.position))
                player = g;
        return player.transform.position;
    }
}
