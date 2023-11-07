using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private Collider2D Collider;

    [Header("Positions")]
    [SerializeField] private Transform normalGunPosition;
    public StateGroup States;

    [Header("UI")]
    [SerializeField] private PlayerUIController UIController;

    void Awake(){

        if(!Input) Input = GetComponent<InputHandler>();
        if(!Anim) Anim = GetComponent<Animator>();
        if(!Renderer) Renderer = GetComponent<SpriteRenderer>();

        Data = Instantiate<PlayerData>(Data);
        Movement = new MovementController(this);
        Config = new PlayerConfigs(1, normalGunPosition);
        StateMachine = new PlayerStateMachine();
        Stats = new PlayerStatsManager(this);

        States = new StateGroup(this, this.StateMachine, this.Data);
    }

    void Start(){
        StateMachine.Initialize(States.IdleState);

        this.Stats.OnDeath += OnDeath;
        this.Stats.OnDamage += OnDamage;
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
        this.Stats.OnDeath -= OnDeath;
        this.Stats.OnDamage -= OnDamage;
    }
    
    void OnDeath(){
        //  
    }

    void OnDamage(Transform origin){
        Vector2 direction = (this.transform.position - origin.position).normalized;
        this.Movement.Rbody().AddForce(direction * 5, ForceMode2D.Impulse);
        this.Movement.Lock(true);
        StartCoroutine(Damaged());
    }

    IEnumerator Damaged(){
        this.Anim.SetTrigger("damage");
        this.DisableCollision();

        this.Renderer.color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);

        this.Movement.Lock(false);

        this.Renderer.color = new Color(255, 255, 255, 0.7f);
        yield return new WaitForSeconds(0.15f);
        this.Renderer.color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);
        this.Renderer.color = new Color(255, 255, 255, 0.7f);
        yield return new WaitForSeconds(0.15f);
        this.Renderer.color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);
        this.Renderer.color = new Color(255, 255, 255, 1f);

        yield return new WaitForSeconds(0.5f);
        this.EnableCollision();
    }

    public PlayerUIController UI() => this.UIController;
    public void EnableCollision() => this.gameObject.layer = 6;
    public void DisableCollision() => this.gameObject.layer = 24;
    public SpriteRenderer Graphics() => this.Renderer;
    public InputHandler GetInput() => this.Input;
}