using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseController : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private Player playerWhoPressed;
    [SerializeField] bool isPaused = false;
    void Update()
    {
        if(!isPaused){
            for(int i = 0; i < PlayerManager.Instance.players.Length; i++)
                if(PlayerManager.Instance.players[i].GetInput().GetPauseDown()){
                    this.playerWhoPressed = PlayerManager.Instance.players[i];
                    PauseUI.SetActive(true);
                    this.isPaused = true;
                    for(int j = 0; j < PlayerManager.Instance.players.Length; j++)
                        PlayerManager.Instance.players[j].GetAIM().Lock(true);
                    Time.timeScale = 0;
                    break;
                }
        }else if(playerWhoPressed && playerWhoPressed.GetInput().GetPauseDown()){
            this.playerWhoPressed = null;
            this.isPaused = false;
            PauseUI.SetActive(false);
            for(int j = 0; j < PlayerManager.Instance.players.Length; j++)
                PlayerManager.Instance.players[j].GetAIM().Lock(false);
            Time.timeScale = 1;
        }
    }
}
