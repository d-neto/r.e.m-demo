using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public GameObject[] totalPlayers;
    public List<GameObject> playersInGame;
    public List<Player> players;
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
        for(int i = 0; i < playersInGame.Count; i++)
            if(!playersInGame[i].GetComponent<Player>().Stats.IsDead()){
                isAllDead = false;
                break;
            }else playersInGame.RemoveAt(i);
        return isAllDead;
    }

    public void ShowGameOver(){
        if(IsAllDead()) UIGameOver.SetActive(true);
    }

    void SetPlayersAlive(ref List<Player> playersComponent, ref List<GameObject> playersObject){
        List<Player> players = new List<Player>();
        List<GameObject> playersObj = new List<GameObject>();

        for(int i = 0; i < totalPlayers.Length; i++)
            if(!totalPlayers[i].GetComponent<Player>().Stats.IsDead()){
                players.Add(totalPlayers[i].GetComponent<Player>());
                playersObj.Add(totalPlayers[i]);
            }
        playersComponent = players;
        playersObject = playersObj;
    }
}
