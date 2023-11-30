using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableComponent : MonoBehaviour {
    
    [Header("Pick. Config.")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject pickableViewObject;
    [SerializeField] private float dropForce = 100f;
    [SerializeField] private bool canDrop = true;
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioSource Audio;

    public enum ComponentType {
        GUN,
        OBJECT
    };
    [SerializeField] private ComponentType objectType;

    PlayerConfigs playerConfigs;

    void Awake(){
        if(!this.Audio) this.Audio = GetComponent<AudioSource>();
    }

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
        Audio.PlayOneShot(pickSound, 0.5f);
    }
    public void OnDrop(){
        // Do something
        GameObject cloneView = Instantiate(pickableViewObject, transform.position, Quaternion.identity);
        this.transform.SetParent(cloneView.transform);
        this.gameObject.SetActive(false);

        Vector3 direction = (player.GetAIM().GetScreenPosition() - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        Rigidbody2D rb = cloneView.GetComponent<Rigidbody2D>();
        rb.AddTorque(100);
        rb.AddForce(direction * dropForce, ForceMode2D.Impulse);
        cloneView.GetComponent<PickableObject>().SetPick(this.gameObject);

        if(GetComponentType() == ComponentType.GUN){
            player.Config?.RemoveFireGun(this.GetComponent<Gun>());
            for(int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }
        GlobalAudio.Instance.Auxiliar().PlayOneShot(dropSound, 0.5f);
    }

    public ComponentType GetComponentType(){
        return this.objectType;
    }

    public void SetPlayer(Player player) => this.player = player;

}