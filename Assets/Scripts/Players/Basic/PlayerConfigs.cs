using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs
{
    [SerializeField] private int fireGunCountInHands = 1;
    [SerializeField] private int gunsInHand = 0;
    [SerializeField] private Transform normalGunPosition;
    [SerializeField] private Transform normalTipsPosition;
    [SerializeField] private Transform nullTargetPosition;
    [SerializeField] private Player player;

    public PlayerConfigs(Player player, int fireGunCountInHands, Transform normalGunPosition, Transform normalTipsPosition, Transform nullTargetPosition){
        this.fireGunCountInHands = fireGunCountInHands;
        this.normalGunPosition = normalGunPosition;
        this.normalTipsPosition = normalTipsPosition;
        this.nullTargetPosition = nullTargetPosition;
        this.player = player;
    }

    public bool CanPickObject(GameObject c){
        if(c.GetComponent<PickableComponent>().GetComponentType() == PickableComponent.ComponentType.GUN){
            return CanPickGun();
        }
        return false;
    }

    public void SetGuns(int value){
        this.gunsInHand = value;
    }

    public bool HasGuns(){
        return this.gunsInHand > 0;
    }

    public bool CanPickGun(){
        return gunsInHand < fireGunCountInHands;
    }
    public Transform GetNormalGunPosition(){
        return normalGunPosition;
    }
    public Transform GetTipPosition(){
        return normalTipsPosition;
    }
    public Transform GetNullTargetPosition(){
        return nullTargetPosition;
    }

    public void AddFireGun(){
        if(gunsInHand<=0) this.gunsInHand = 0;
        this.gunsInHand++;
    }
    public void RemoveFireGun(){
        this.gunsInHand--;
        if(gunsInHand <= 0) player.GetAIM().Disabled(true);
    }
}