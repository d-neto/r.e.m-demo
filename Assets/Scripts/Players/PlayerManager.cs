using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public GameObject[] playersInGame;
    public GameObject UIGameOver;

    void Awake(){
        if(!Instance)
            Instance = this;
        else Destroy(this);
    }

    void Start(){
        this.playersInGame = GameObject.FindGameObjectsWithTag("Player");
    }


    public bool IsAllDead(){
        bool isAllDead = true;

        for(int i = 0; i < playersInGame.Length; i++)
            if(!playersInGame[i].GetComponent<Player>().Stats.IsDead()){
                isAllDead = false;
                break;
            }

        return isAllDead;
    }

    public void ShowGameOver(){
        if(IsAllDead()) UIGameOver.SetActive(true);
    }
}
