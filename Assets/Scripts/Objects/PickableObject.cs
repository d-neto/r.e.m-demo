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

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private SpriteRenderer spriteToHighlight;
    [SerializeField] private PickableComponent pickableComponent;

    bool playerCatch = false;
    // Update is called once per frame
    void Update()
    {
        if(IsPlayerOnArea()){

            if(!playerCatch){
                this.spriteToHighlight.material = highlightMaterial;
                player.GetTips().ShowPick();
                playerCatch = true;
            }

            if(canPick && player.GetInput().GetPickObject()){
                playerConfigs = player.Config;
                if(!playerConfigs.CanPickObject(pickObject))
                    return;
                pickObject.transform.SetParent(playerConfigs.GetNormalGunPosition());
                pickableComponent.OnPick(this.gameObject, player);
                pickableComponent.SetPlayer(player);
                playerConfigs.AddFireGun();
                player.GetTips().Destroy();
            }
        }else if(playerCatch){
            this.spriteToHighlight.material = defaultMaterial;
            if(player)
                player.GetTips().Destroy();
            playerCatch = false;
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

    public void SetPick(GameObject pick){
        this.pickObject = pick;
        this.pickableComponent = this.pickObject.GetComponent<PickableComponent>() ? this.pickObject.GetComponent<PickableComponent>() : null;
        if(this.pickableComponent == null){
            Debug.LogError("Invalid Pickable Object!");
            canPick = false;
        }
        this.pickObject.SetActive(false);
    }
}
