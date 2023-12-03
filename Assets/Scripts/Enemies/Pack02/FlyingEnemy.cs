using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] AudioSource Audio;
    [SerializeField] AudioClip damageAudioClip;
    [SerializeField] float volume = 0.4f;
    [SerializeField] Animator SuperAnimator;
    [SerializeField] GameObject UIDamageIndicatorText;
    [SerializeField] GameObject DamageCanvas;
    [SerializeField] Transform particleSpawn;
    [SerializeField] Transform XpTarget;
    [SerializeField] GameObject PSDeath;
    public float speed = 1f;
    bool isTakingDamage;
    bool isAttacking;
    public override void OnAwake()
    {
        base.OnAwake();
        if(!this.SuperAnimator)
            this.SuperAnimator = GetComponent<Animator>();
        if(!this.Audio)
            this.Audio = transform.GetComponent<AudioSource>();

        DamageCanvas = GameObject.FindWithTag("UI:DamageIndicators");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.rb2D.velocity = (GetTargetPosition() - transform.position).normalized*speed;
        if(Vector2.Distance(transform.position, GetTargetPosition()) < 0.1 && !isAttacking){
            StartCoroutine(Attack());
        }
    }

    protected GameObject actualDamageIndicatorText = null;
    public override void OnDamage(float damage, Vector3 direction){

        if(!actualDamageIndicatorText)
            actualDamageIndicatorText = Instantiate(UIDamageIndicatorText, DamageCanvas.transform);

        actualDamageIndicatorText.transform.position = this.transform.position;
        actualDamageIndicatorText.GetComponent<TMP_Text>().text = ((int) damage).ToString();
        actualDamageIndicatorText.GetComponent<Animator>().Play("show");

        if(damage >= this.Data.life) actualDamageIndicatorText.GetComponent<TMP_Text>().text = "Dead!";

        isTakingDamage = true;
        Audio.PlayOneShot(damageAudioClip, volume);
        StartCoroutine(TakeDamage(direction));
        if(damage - Data.endurance >= 0)
            this.Data.life -= (damage - Data.endurance);
    }


    public virtual IEnumerator TakeDamage(Vector3 direction){
        SuperAnimator.SetTrigger("damage");
        Anim.SetTrigger("damage");
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(direction * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.2f);
        rb2D.velocity = Vector2.zero;
        isTakingDamage = false;
    }

    public virtual IEnumerator Attack(){
        isAttacking = true;
        SuperAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(0.2f);
        gameObject.layer = 17;
        yield return new WaitForSeconds(2f);
        gameObject.layer = 18;
        isAttacking = false;
    }

    GameObject clone;
    public override void OnDeath(){
        SuperAnimator.SetTrigger("dead");
        Anim.SetTrigger("dead");
        Instantiate(PSDeath, particleSpawn.position, Quaternion.identity);
        clone = Instantiate(GetData().XPPrefab, XpTarget.position, GetComponent<AbleToAim>().Get().rotation);
        clone.GetComponent<Experience>().Set(GetData().XPAmount);
        base.OnDeath();
    }
}
