using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MeleeEnemy
{


    [Header("Configs.")]
    [SerializeField] private float waitTime = 0.9f;
    [SerializeField] private float airTime = 0.9f;
    bool startCoroutine = false;
    bool isJumping = false;
    bool isTakingDamage = false;

    [Header("Components")]
    [SerializeField] protected Animator SuperAnimator;
    [SerializeField] protected GameObject PSDeath;
    [SerializeField] protected Transform particleSpawn;
    [SerializeField] protected AudioSource Audio;

    [Header("Audios")]
    [SerializeField] private AudioClip jumpAudioClip;
    [SerializeField] private AudioClip damageAudioClip;
    [SerializeField] private AudioClip deathAudioClip;

    public override void OnAwake()
    {
        base.OnAwake();
        if(!this.SuperAnimator)
            this.SuperAnimator = GetComponent<Animator>();
        if(!this.Audio)
            this.Audio = transform.GetComponent<AudioSource>();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if(isJumping && !isTakingDamage) MoveToTarget();

        if(!startCoroutine && !isTakingDamage){
            StartCoroutine(MovingToTarget());
        }
    }

    public override void OnDeath(){
        if(this.isDead) return;
        SuperAnimator.SetTrigger("dead");
        Anim.SetTrigger("dead");
        Instantiate(PSDeath, particleSpawn.position, Quaternion.identity);
        this.isDead = true;
    }

    public override void OnDamage(float damage, Vector3 direction){
        isTakingDamage = true;
        Audio.PlayOneShot(damageAudioClip, 0.3f);
        StartCoroutine(TakeDamage(direction));
        if(damage - Data.endurance >= 0)
            this.Data.life -= (damage - Data.endurance);
    }

    void MoveToTarget(){
        Vector2 direction = (target.position - transform.position).normalized;
        rb2D.velocity = direction * Data.speed;
    }

    public void KillSlime(){
        Audio.PlayOneShot(deathAudioClip, 0.3f);
        Destroy(this.gameObject, 2f);
    }

    IEnumerator MovingToTarget(){
        startCoroutine = true;
        Audio.PlayOneShot(jumpAudioClip, 0.3f);
        rb2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(waitTime);
        SuperAnimator.SetBool("jump", true);
        Anim.SetBool("jump", true);
        yield return new WaitForSeconds(0.2f);

        isJumping = true;

        yield return new WaitForSeconds(airTime);
        SuperAnimator.SetBool("jump", false);
        Anim.SetBool("jump", false);
        yield return new WaitForSeconds(0.1f);

        isJumping = false;
        startCoroutine = false;
    }

    IEnumerator TakeDamage(Vector3 direction){

        isJumping = false;
        SuperAnimator.SetTrigger("damage");
        Anim.SetTrigger("damage");
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(direction * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.2f);
        rb2D.velocity = Vector2.zero;
        isTakingDamage = false;

    }
}
