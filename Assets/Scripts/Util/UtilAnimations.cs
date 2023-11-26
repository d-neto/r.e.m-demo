using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilAnimations : MonoBehaviour
{

    public void DisableObject(){
        this.gameObject.SetActive(false);
    }

    public void DestroyObject(){
        Destroy(this.gameObject);
    }

    public void DestroyObjectWithTiming(float time){
        Destroy(this.gameObject, time);
    }

}
