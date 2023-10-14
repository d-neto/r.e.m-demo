using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{

    public PlayerStateMachine StateMachine {get; private set;}
    public MovementController Movement {get; private set;}
    public PlayerConfigs Config {get; private set;}
    [Header("Player Classes")]
    public PlayerData Data;

    [Header("Player Components")]
    [SerializeField] private InputHandler Input;
    [SerializeField] private Animator Anim;

    [Header("Positions")]
    [SerializeField] private Transform normalGunPosition;
    public StateGroup States;

    void Awake(){

        if(!Input) Input = GetComponent<InputHandler>();

        Movement = new MovementController(GetComponent<Rigidbody2D>(), Data.speed);
        Config = new PlayerConfigs(1, normalGunPosition);
        StateMachine = new PlayerStateMachine();

        States = new StateGroup(this, this.StateMachine, this.Data);
    }

    void Start(){
        if(!Anim) Anim = GetComponent<Animator>();
        StateMachine.Initialize(States.IdleState);
    }

    void Update(){
        Movement.Update();
        StateMachine.CurrentState?.Update();
    }

    void FixedUpdate(){
        StateMachine.CurrentState?.FixedUpdate();
    }
    
    public Animator GetAnimator() => this.Anim;

}