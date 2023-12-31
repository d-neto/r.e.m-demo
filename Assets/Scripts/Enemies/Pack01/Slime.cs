using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slime : Enemy
{


    [Header("Configs.")]
    [SerializeField] protected float waitTime = 0.9f;
    [SerializeField] protected float airTime = 0.9f;
    protected bool startCoroutine = false;
    protected bool isJumping = false;
    protected bool isTakingDamage = false;

    [Header("UI")]
    [SerializeField] protected static GameObject DamageCanvas;
    [SerializeField] protected GameObject UIDamageIndicatorText;

    [Header("Components")]
    [SerializeField] protected Animator SuperAnimator;
    [SerializeField] protected GameObject PSDeath;
    [SerializeField] protected Transform particleSpawn;
    [SerializeField] protected AudioSource Audio;

    [Header("Audios")]
    [SerializeField] protected float volume;
    [SerializeField] protected AudioClip jumpAudioClip;
    [SerializeField] protected AudioClip damageAudioClip;
    [SerializeField] protected AudioClip deathAudioClip;

    public override void OnAwake()
    {
        base.OnAwake();
        if(!this.SuperAnimator)
            this.SuperAnimator = GetComponent<Animator>();
        if(!this.Audio)
            this.Audio = transform.GetComponent<AudioSource>();

        this.waitTime = Random.Range(this.waitTime-0.1f, this.waitTime+0.1f);

        DamageCanvas = GameObject.FindWithTag("UI:DamageIndicators");
        Finder.StartUpdate();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Finder.Update();

        if(isJumping && !isTakingDamage) MoveToTarget();

        if(!startCoroutine && !isTakingDamage){
            StartCoroutine(MovingToTarget());
        }
    }

    GameObject clone;
    public override void OnDeath(){
        base.OnDeath();
        SuperAnimator.SetTrigger("dead");
        Anim.SetTrigger("dead");
        Instantiate(PSDeath, particleSpawn.position, Quaternion.identity);
        clone = Instantiate(GetData().XPPrefab, GetComponent<AbleToAim>().Get().position, GetComponent<AbleToAim>().Get().rotation);
        clone.GetComponent<Experience>().Set(GetData().XPAmount);
    }

    protected GameObject actualDamageIndicatorText = null;
    public override void OnDamage(float damage, Vector3 direction){

        if(!actualDamageIndicatorText)
            actualDamageIndicatorText = Instantiate(UIDamageIndicatorText, DamageCanvas.transform);

        actualDamageIndicatorText.transform.position = this.transform.position;
        actualDamageIndicatorText.GetComponent<TMP_Text>().text = ((int) damage).ToString();
        actualDamageIndicatorText.GetComponent<Animator>().Play("show");

        if(damage >= this.Data.life) actualDamageIndicatorText.GetComponent<TMP_Text>().text = "NaN";

        isTakingDamage = true;
        Audio.PlayOneShot(damageAudioClip, volume);
        StartCoroutine(TakeDamage(direction));
        if(damage - Data.endurance >= 0)
            this.Data.life -= (damage - Data.endurance);
    }

    protected void MoveToTarget(){
        // Vector2 direction = (target.position - transform.position).normalized;
        rb2D.velocity = Finder.GetDirection() * Data.speed;
    }

    public virtual IEnumerator MovingToTarget(){
        startCoroutine = true;
        Audio.PlayOneShot(jumpAudioClip, volume);
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

    public virtual IEnumerator TakeDamage(Vector3 direction){

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
