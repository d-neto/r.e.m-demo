using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager
{

    public delegate void StatEvent(Player player);
    public static event StatEvent OnDeath;
    public static event StatEvent OnDamage;

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
            OnDeath?.Invoke(player);
            isDead = true;
        }
    }

    public void TakeDamage(float damage){
        player.Data.life -= (damage - player.Data.endurance) > 0 ? (damage-player.Data.endurance) : 0;
        LifeSlider(player.Data.life);
        OnDamage?.Invoke(player);
    }

    void LifeSlider(float life){
        if(lifeSlider)
            lifeSlider.value = life;
    }
}
