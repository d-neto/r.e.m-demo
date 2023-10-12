using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{

    public PlayerStateMachine StateMachine {get; private set;}
    public MovementController Movement {get; private set;}
    public PlayerConfigs Config {get; private set;}

    public PlayerData Data;

    [Header("Positions")]
    [SerializeField] private Transform normalGunPosition;

    void Awake(){
        Movement = new MovementController(GetComponent<Rigidbody2D>(), Data.speed);
        Config = new PlayerConfigs(1, normalGunPosition);
    }

    void Update(){
        Movement.Update();
    }

}