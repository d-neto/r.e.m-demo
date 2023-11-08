using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTips
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject pickObjectButton;

    public PlayerTips(){}
    public PlayerTips(Player player){
        this.player = player;
    }

    public GameObject PickButton() => this.pickObjectButton;

    GameObject actualTip;
    public void Show(GameObject tip){
        if(actualTip && actualTip != tip) GameObject.Destroy(actualTip);
        actualTip = GameObject.Instantiate(tip, player.Config.GetTipPosition().position, player.Config.GetTipPosition().rotation);
        actualTip.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void ShowPick(){
        if(actualTip == pickObjectButton) return;
        Show(pickObjectButton);
    }

    public void Destroy(){
        if(actualTip) GameObject.Destroy(actualTip);
    }

}
