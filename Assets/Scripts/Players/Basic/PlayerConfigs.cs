using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs
{
    [SerializeField] private int maxFireGunCountInHands = 1;
    [SerializeField] private Transform normalGunPosition;
    [SerializeField] private Transform normalTipsPosition;
    [SerializeField] private Transform nullTargetPosition;
    [SerializeField] private List<Gun> guns = new List<Gun>();
    [SerializeField] private Player player;

    public PlayerConfigs(Player player, int maxFireGunCountInHands, Transform normalGunPosition, Transform normalTipsPosition, Transform nullTargetPosition){
        this.maxFireGunCountInHands = maxFireGunCountInHands;
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

    public bool HasGuns(){
        return this.guns.Count > 0;
    }

    public bool CanPickGun(){
        return guns.Count < maxFireGunCountInHands;
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

    public void AddFireGun(Gun gun){
        if(gun == null) return;
        guns.Add(gun);
    }
    public void RemoveFireGun(Gun gun){
        if(gun == null) return;
        guns.Remove(gun);
        if(guns.Count <= 0) player.GetAIM().Disabled(true);
    }

    public void DropAllGuns(){
        for(int i = 0; i < guns.Count; i++){
            guns[i].PickObject().OnDrop();
        }
        guns.Clear();
        if(guns.Count <= 0) player.GetAIM().Disabled(true);
    }
}