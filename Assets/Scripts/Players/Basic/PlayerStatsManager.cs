using System.Collections;
using System.Collections.Generic;
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
    private Slider skillPowerSlider;
    private bool isDead = false;
    public PlayerStatsManager(Player player){
        this.player = player;
        this.lifeSlider = player.UI().LifeSlider();
        this.skillPowerSlider = player.UI().SkillSlider();

        if(this.lifeSlider){
            this.lifeSlider.maxValue = player.Data.life;
            this.lifeSlider.value = player.Data.life;
        }
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
        OnDamage?.Invoke(origin);
    }

    void LifeSlider(float life){
        if(lifeSlider)
            lifeSlider.value = life;
    }

    public void AddXp(float amount){
        this.player.Data.experience += amount;
    }

    public bool IsDead() => isDead;
}
