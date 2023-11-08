using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{

    public static GlobalAudio Instance;

    public AudioSource backgroundAudioSource;
    public AudioSource auxiliarAudio;

    void Awake(){
        if(!Instance)
            Instance = this;
        else Destroy(this);
    }


    public AudioSource Auxiliar() => this.auxiliarAudio;
}
