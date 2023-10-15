using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager
{

    public delegate void StatEvent(Player player);
    public static event StatEvent OnDeath;


    private Player player;
    private Slider lifeSlider;
    private Slider skillPowerSlider;

    public PlayerStatsManager(Player player){
        this.player = player;
        this.lifeSlider = player.UI().LifeSlider();
        this.skillPowerSlider = player.UI().SkillSlider();
    }

    public void Update(){
        if(player.Data.life <= 0) OnDeath(player);
    }

    public void TakeDamage(float damage){
        player.Data.life -= (damage - player.Data.endurance) > 0 ? (damage-player.Data.endurance) : 0;
        LifeSlider(player.Data.life);
    }

    void LifeSlider(float life){
        if(lifeSlider)
            lifeSlider.value = life;
    }
}
