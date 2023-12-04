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
    public List<Transform> SpawnPositions = new List<Transform>();
    void Awake(){
        if(!Instance)
            Instance = this;
        else Destroy(this);

        for(int i = 0; i < GameManager.Instance.players.Count; i++){
            GameObject player = Instantiate(GameManager.Instance.players[i], null);
            player.transform.position = SpawnPositions[i].position;
            Player playerComponent = player.GetComponent<Player>();
            playerComponent.GetInput().Set(GameManager.Instance.playersInputs[i]);
            playerComponent.GetAIM().mode = GameManager.Instance.playersInputs[i].targetMode;
            playerComponent.GetAIM().ActiveMode();
            playersInGame.Add(player);
        }
    }

    void Start(){
        this.totalPlayers = playersInGame.ToArray();
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
