using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public GameObject[] totalPlayers;
    public GameObject[] playersInGame;
    public Player[] players;
    public GameObject UIGameOver;

    void Awake(){
        if(!Instance)
            Instance = this;
        else Destroy(this);
    }

    void Start(){
        this.totalPlayers = GameObject.FindGameObjectsWithTag("Player");
        SetPlayersAlive(ref players, ref playersInGame);
    }


    public bool IsAllDead(){
        SetPlayersAlive(ref players, ref playersInGame);
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

    void SetPlayersAlive(ref Player[] playersComponent, ref GameObject[] playersObject){
        List<Player> players = new List<Player>();
        List<GameObject> playersObj = new List<GameObject>();

        for(int i = 0; i < totalPlayers.Length; i++)
            if(!totalPlayers[i].GetComponent<Player>().Stats.IsDead()){
                players.Add(totalPlayers[i].GetComponent<Player>());
                playersObj.Add(totalPlayers[i]);
            }
        playersComponent = players.ToArray();
        playersObject = playersObj.ToArray();
    }
}
