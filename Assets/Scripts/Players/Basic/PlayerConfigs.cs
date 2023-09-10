using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs : MonoBehaviour
{
    [SerializeField] private int fireGunCountInHands = 1;
    [SerializeField] private int gunsInHand = 0;
    [SerializeField] private Transform normalGunPosition;

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

    public void AddFireGun(){
        this.gunsInHand++;
    }
    public void RemoveFireGun(){
        this.gunsInHand--;
    }
}
