using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager
{

    public delegate void StatEvent();
    public delegate void DamageEvent(Transform origin);
    public event StatEvent OnDeath;
    public event DamageEvent OnDamage;

    private Player player;
    private Slider lifeSlider;
    private Slider XPSlider;
    private Slider skillPowerSlider;
    private TMP_Text level;
    private bool isDead = false;
    private float initialMaxExperience = 10;
    private float maxExperience = 10;
    private float maxLife = 0;
    public PlayerStatsManager(Player player){
        this.player = player;
        this.lifeSlider = player.UI().LifeSlider();
        this.XPSlider = player.UI().ExpSlider();
        this.skillPowerSlider = player.UI().SkillSlider();
        this.maxLife = player.Data.life;
        this.level = player.UI().LevelText();
        // if(this.lifeSlider){
        //     this.lifeSlider.maxValue = player.Data.life;
        //     this.lifeSlider.value = player.Data.life;
        // }
        // if(this.XPSlider != null)
        //     VerifyXP();
    }

    public void Update(){
        if(isDead) return;

        if(player.Data.life <= 0){
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void TakeDamage(float damage, Transform origin){
        player.Data.life -= (damage - player.Data.endurance) > 0 ? (damage-player.Data.endurance) : 0;
        LifeSlider(player.Data.life);
        player.UI().LifeSliderAnimator().SetTrigger("damage");
        if(player.Data.life <= 0){
            isDead = true;
            OnDeath?.Invoke();
        }else OnDamage?.Invoke(origin);
    }

    void LifeSlider(float life){
        if(lifeSlider)
            lifeSlider.value = life;
    }

    public void AddXp(float amount){
        this.player.Data.experience += amount;
        VerifyXP();
    }

    public void VerifyXP(){ 
        while(player.Data.experience >= maxExperience){
            player.Data.level++;
            maxExperience = CalculateNewMaxXP(player.Data.level);
            this.XPSlider.minValue = player.Data.experience;
            if(SkillSelector.Instance)
                SkillSelector.Instance.StartSelector(player);
        }
        float gap = (maxExperience-this.XPSlider.minValue) * 0.08f;
        this.XPSlider.maxValue = maxExperience + gap;
        this.XPSlider.value = player.Data.experience + gap;
        this.level.text = player.Data.level.ToString();
    }

    public void UpdateLifeSlider(){
        if(this.lifeSlider){
            this.lifeSlider.maxValue = maxLife;
            this.lifeSlider.value = player.Data.life;
        }
    }

    float CalculateNewMaxXP(int actualLevel){
        return maxExperience + initialMaxExperience + (actualLevel*2f);
    }

    public bool IsDead() => isDead;
}
