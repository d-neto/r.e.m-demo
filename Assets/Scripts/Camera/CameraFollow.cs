using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public GameObject[] players;
    public Transform centerPoint;
    public float maxDistance = 4f;

    void Awake(){
        if(!Instance) Instance = this;
        else Destroy(this);

        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        centerPoint.position = Vector3.zero;
        for(int i = 0; i < players.Length; i++){
            centerPoint.position += players[i].transform.position;
        }
        centerPoint.position /= players.Length;
    }

}
