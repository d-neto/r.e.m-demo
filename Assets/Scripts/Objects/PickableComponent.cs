using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableComponent : MonoBehaviour {
    
    [Header("Pick. Config.")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject pickableViewObject;
    [SerializeField] private float dropForce = 100f;
    [SerializeField] private bool canDrop = true;

    public enum ComponentType {
        GUN,
        OBJECT
    };
    [SerializeField] private ComponentType objectType;

    PlayerConfigs playerConfigs;

    void Update(){
        if(canDrop && player.GetInput().GetDropObject())
            OnDrop();
    }

    public void OnPick(GameObject originViewObject, Player player){
        // Do something...
        Destroy(originViewObject);
        this.playerConfigs = player.Config;
        this.gameObject.transform.localPosition = Vector3.zero;
    
        if(this.objectType == ComponentType.GUN)
            this.GetComponent<Gun>().withPlayer = player;

        this.gameObject.SetActive(true);
    }
    public void OnDrop(){
        // Do something
        GameObject cloneView = Instantiate(pickableViewObject, transform.position, Quaternion.identity);
        this.transform.SetParent(cloneView.transform);
        this.gameObject.SetActive(false);

        Vector3 direction = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        cloneView.GetComponent<Rigidbody2D>().AddTorque(100);
        cloneView.GetComponent<Rigidbody2D>().AddForce(direction * dropForce, ForceMode2D.Impulse);
        
        if(GetComponentType() == ComponentType.GUN)
            playerConfigs?.RemoveFireGun();
    }

    public ComponentType GetComponentType(){
        return this.objectType;
    }

    public void SetPlayer(Player player) => this.player = player;

}