using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    
    public static XPManager Instance;
    [SerializeField] private AudioSource Audio;

    void Awake(){
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);

        if(!Audio) Audio = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioclip, float volume = 0.5f) => this.Audio.PlayOneShot(audioclip, volume);
}
