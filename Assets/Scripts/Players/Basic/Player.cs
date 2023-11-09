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
    public PlayerTips Tips;

    [Header("Player Components")]
    [SerializeField] private InputHandler Input;
    [SerializeField] private Animator Anim;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private Collider2D Collider;
    [SerializeField] private AimController AIM;

    [Header("Positions")]
    [SerializeField] private Transform normalGunPosition;
    [SerializeField] private Transform nullTargetPosition;
    [SerializeField] private Transform normalTipPosition;
    public StateGroup States;

    [Header("UI")]
    [SerializeField] private PlayerUIController UIController;

    void Awake(){

        if(!Input) Input = GetComponent<InputHandler>();
        if(!Anim) Anim = GetComponent<Animator>();
        if(!Audio) Audio = GetComponent<AudioSource>();
        if(!Renderer) Renderer = GetComponent<SpriteRenderer>();

        Data = Instantiate<PlayerData>(Data);
        Movement = new MovementController(this);
        Config = new PlayerConfigs(this, Data.maxGunsInHands, normalGunPosition, normalTipPosition, nullTargetPosition);
        StateMachine = new PlayerStateMachine();
        Stats = new PlayerStatsManager(this);
        Tips ??= new PlayerTips(this);

        States = new StateGroup(this, this.StateMachine, this.Data);

        Config.SetGuns(Data.actualGunsInHands);
        CreateAim();
    }

    void Start(){
        StateMachine.Initialize(States.IdleState);

        this.Stats.OnDeath += OnDeath;
        this.Stats.OnDamage += OnDamage;

        this.AIM.gameObject.SetActive(true);
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
        PlayAudio(Data.ACHurt, 0.5f);
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
    public void PlayAudio(AudioClip audioClip, float volume = 0.4f) => this.Audio.PlayOneShot(audioClip, volume);
    public PlayerTips GetTips() => this.Tips;
    public AimController GetAIM() => this.AIM;
    public void CreateAim(){
        this.AIM = Instantiate(this.Data.AimPrefab, null).GetComponent<AimController>();
        this.AIM.Setup(this, Config.GetNullTargetPosition());
    }
}