using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private Player forPlayer = null;
    [SerializeField] private AudioClip audioClip = null;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Player")){
            if(!forPlayer){
                other.gameObject.GetComponent<Player>().Stats.AddXp(this.amount);
                Destroy(this.gameObject);
                XPManager.Instance.PlayAudio(audioClip);
            }else if(forPlayer == other.gameObject.GetComponent<Player>()){
                forPlayer.Stats.AddXp(this.amount);
                XPManager.Instance.PlayAudio(audioClip);
                Destroy(this.gameObject);
            }
        }

    }

    public float Get() => this.amount;
    public void SetPlayer(Player p) => this.forPlayer = p;
}
