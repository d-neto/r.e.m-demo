using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs
{
    [SerializeField] private int fireGunCountInHands = 1;
    [SerializeField] private int gunsInHand = 0;
    [SerializeField] private Transform normalGunPosition;
    [SerializeField] private Transform normalTipsPosition;

    public PlayerConfigs(int fireGunCountInHands, Transform normalGunPosition, Transform normalTipsPosition){
        this.fireGunCountInHands = fireGunCountInHands;
        this.normalGunPosition = normalGunPosition;
        this.normalTipsPosition = normalTipsPosition;
    }

    public bool CanPickObject(GameObject c){
        if(c.GetComponent<PickableComponent>().GetComponentType() == PickableComponent.ComponentType.GUN){
            return CanPickGun();
        }
        return false;
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

    public void AddFireGun(){
        this.gunsInHand++;
    }
    public void RemoveFireGun(){
        this.gunsInHand--;
    }
}
