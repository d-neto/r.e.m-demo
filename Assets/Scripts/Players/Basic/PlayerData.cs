using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{

    [Header("Stats")]
    public float life;
    public float endurance;
    public float skillPower;
    public int level = 0;
    public float experience = 0;
    public float score = 0;

    [Header("Movement")]
    public float speed;

    [Header("Sounds")]
    public AudioClip ACHurt;
    public AudioClip ACDash;
    public AudioClip ACDead;

    [Header("Prefabs")]
    public GameObject AimPrefab;
    public GameObject PSDeathPrefab;
    
    [Header("Hand Weapons")]
    public int maxGunsInHands = 1;
    public GameObject actualGunInHands = null;
    public List<Weapon> weapons;


    public GameObject GetWeapon(string code){
        GameObject found = weapons[0].source;
        foreach(Weapon weapon in this.weapons)
            if(weapon.referenceCode == code){
                found = weapon.source;
                break;
            }
        return found;
    }
}
[System.Serializable]
public class Weapon{
    public string referenceCode;
    public GameObject source;
}