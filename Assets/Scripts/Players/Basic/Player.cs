using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{

    public PlayerStateMachine StateMachine {get; private set;}
    public MovementController Movement {get; private set;}
    public PlayerConfigs Config {get; private set;}
    public PlayerStatsManager Stats {get; private set;}
    [Header("Player Classes")]
    public PlayerData Data;

    [Header("Player Components")]
    [SerializeField] private InputHandler Input;
    [SerializeField] private Animator Anim;

    [Header("Positions")]
    [SerializeField] private Transform normalGunPosition;
    public StateGroup States;

    [Header("UI")]
    [SerializeField] private PlayerUIController UIController;

    void Awake(){

        if(!Input) Input = GetComponent<InputHandler>();

        Movement = new MovementController(GetComponent<Rigidbody2D>(), Data.speed);
        Config = new PlayerConfigs(1, normalGunPosition);
        StateMachine = new PlayerStateMachine();
        Stats = new PlayerStatsManager(this);

        States = new StateGroup(this, this.StateMachine, this.Data);
    }

    void Start(){
        if(!Anim) Anim = GetComponent<Animator>();
        StateMachine.Initialize(States.IdleState);

        PlayerStatsManager.OnDeath += OnDeath;
        PlayerStatsManager.OnDamage += OnDamage;
    }

    void Update(){
        Stats.Update();
        Movement.Update();
        StateMachine.CurrentState?.Update();
    }

    void FixedUpdate(){
        StateMachine.CurrentState?.FixedUpdate();
    }
    
    public Animator GetAnimator() => this.Anim;

    private void OnDestroy() {
        PlayerStatsManager.OnDeath -= OnDeath;
        PlayerStatsManager.OnDamage -= OnDamage;
    }
    
    void OnDeath(Player player){
        if(player == this) Debug.Log("DEAD!");
    }

    void OnDamage(Player player){
        if(player == this) Debug.Log("DAMAGE!");
    }

    public PlayerUIController UI() => this.UIController;

}