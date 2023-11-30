using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public Transform centerPoint;
    public float maxDistance = 9f;

    void Awake(){
        if(!Instance) Instance = this;
        else Destroy(this);
    }

    void Update()
    {
        centerPoint.position = Vector3.zero;
        for(int i = 0; i < PlayerManager.Instance.playersInGame.Count; i++){
            centerPoint.position += PlayerManager.Instance.playersInGame[i].transform.position;
        }
        centerPoint.position /= PlayerManager.Instance.playersInGame.Count > 0 ? PlayerManager.Instance.playersInGame.Count : 1;
    }

}
