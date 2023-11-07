using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{


    [SerializeField] private GameObject pickObject;
    [SerializeField] private float area;
    [SerializeField] private LayerMask whatIsPlayer;
    private Player player;
    private PlayerConfigs playerConfigs;
    private bool canPick = true;

    private PickableComponent pickableComponent;
    void Start()
    {
        if(this.transform.GetChild(1)){
            pickObject = this.transform.GetChild(1).gameObject;
            pickObject.SetActive(false);
            pickableComponent = pickObject.GetComponent<PickableComponent>();
            if(pickableComponent == null){
                Debug.LogError("Invalid Pickable Object!");
                canPick = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canPick && IsPlayerOnArea() && player.GetInput().GetPickObject()){
            playerConfigs = player.Config;

            if(!playerConfigs.CanPickObject(pickObject))
                return;
                
            pickObject.transform.SetParent(playerConfigs.GetNormalGunPosition());
            pickableComponent.OnPick(this.gameObject, player);
            pickableComponent.SetPlayer(player);
            playerConfigs.AddFireGun();
        }
    }

    bool IsPlayerOnArea(){
        Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position, area, whatIsPlayer);
        if(detections != null && detections.Length > 0){
            player = detections[0].GetComponent<Player>();
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
