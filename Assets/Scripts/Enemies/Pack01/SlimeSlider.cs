using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SlimeSlider : Slime
{

    float scaleX;
    Vector3 scale;

    public override void OnAwake()
    {
        base.OnAwake();
        scaleX = transform.localScale.x;
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Finder.SetTimer(0.1f);
    }
    public override void OnUpdate()
    {
        Finder.Update();

        if((transform.position.x - GetTargetPosition().x) > 0){
            scale.x = scaleX;
        }else if((transform.position.x - GetTargetPosition().x) < 0){
            scale.x = -scaleX;
        }
        transform.localScale = scale;

        if(!isTakingDamage) MoveToTargetWithAnim();
    }


    protected void MoveToTargetWithAnim(){
        Anim.SetBool("jump", true);
        SuperAnimator.SetBool("jump", true);
        rb2D.velocity = Finder.GetDirection() * Data.speed;
    }

    public override void OnDamage(float damage, Vector3 direction){

        if(!actualDamageIndicatorText)
            actualDamageIndicatorText = Instantiate(UIDamageIndicatorText, DamageCanvas.transform);

        actualDamageIndicatorText.transform.position = this.transform.position;
        actualDamageIndicatorText.GetComponent<TMP_Text>().text = ((int) damage).ToString();
        actualDamageIndicatorText.GetComponent<Animator>().Play("show");

        if(damage >= this.Data.life) actualDamageIndicatorText.GetComponent<TMP_Text>().text = "DEAD!";

        isTakingDamage = true;
        Audio.PlayOneShot(damageAudioClip, volume);
        StartCoroutine(TakeDamage(direction));
        if(damage - Data.endurance >= 0)
            this.Data.life -= (damage - Data.endurance);
    }

}
