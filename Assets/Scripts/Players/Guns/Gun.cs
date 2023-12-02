using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Player withPlayer;

    [Header("Configs.")]
    [SerializeField] private float maxAngle = 360.0f;
    [SerializeField] private Transform targetRefer;
    [SerializeField] protected AudioSource Audio;
    [SerializeField] protected PickableComponent pickableComponent;
    [SerializeField] private bool canInvertOn90Degree = true;

    [Header("Audios")]
    [SerializeField] protected AudioClip shootAudioClip;
    [SerializeField] protected AudioClip reloadAudioClip;
    [SerializeField] protected AudioClip emptyAudioClip;

    [Header("Attributes")]
    [SerializeField] protected string referenceCode;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int loadedAmmo;

    [SerializeField] private int fireRateInMS;
    [SerializeField] private int reloadTimeInMS;
    [SerializeField] protected Transform bulletTarget;
    [SerializeField] protected AimController Aim;

    [Header("Lights")]
    [SerializeField] protected bool hasLight;
    [SerializeField] protected GameObject LightPrefab;
    [SerializeField] protected GameObject Light;

    protected float reloadTime;
    protected float fireRate;

    Vector3 positionRefer;
    Vector3 rotateToTargetRefer;
    Vector3 mouseDirection;
    protected Vector2 BulletToDirection;
    protected float angle;
    float originalScaleY;

    private void Awake(){
        if(!this.Audio)
            this.Audio = GetComponent<AudioSource>();
        if(!this.pickableComponent) 
            this.pickableComponent = GetComponent<PickableComponent>();
    }

    private void Start(){
        reloadTime = (float) reloadTimeInMS / 1000f;
        fireRate = (float) fireRateInMS / 1000f;

        originalScaleY = transform.localScale.y;

        if(targetRefer == null) targetRefer = transform;

        OnStart();
    }

    private void OnEnable(){
        if(!withPlayer) return;
        if(this.transform.childCount <= 0)
            this.SetupGun(withPlayer.Data.GetWeapon(referenceCode), withPlayer.GetAIM());
        if(hasLight){
            if(this.Light) Destroy(this.Light);
            this.Light = Instantiate(this.LightPrefab, null);
            this.Light.transform.SetParent(bulletTarget);
            this.Light.transform.localPosition = Vector3.zero;
        }
        OnEnableGun();
    }

    private void Update()
    {
        ApplyRotation();
        ShootingController();
    }

    public virtual void ShootingController(){
        // Method called on every frame
        // Define when player press buttons and shoot actions
    }
    public virtual void Shoot(){
        // Especify what happens on shoot 
        // that will called on ShootingController.
    }
    public virtual void Reload(){
        // Especify what happens when the 
        // player reload an weapon
    }

    public virtual void OnEnableGun() {
        
    }
    public virtual void OnStart() {
        if(withPlayer == null) return;
        this.Aim = withPlayer.GetAIM();
    }

    public virtual void ApplyRotation(){

        if(!Aim.GetActualTarget()){
            return;
        }

        BulletToDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        positionRefer = Camera.main.WorldToScreenPoint(targetRefer.position);
        mouseDirection =  Aim.GetScreenPosition() - positionRefer;

        this.withPlayer.Movement.SetActualTarget(Aim.GetActualTarget());
        this.BulletToDirection = Aim.GetActualTarget().position;

        angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(canInvertOn90Degree){
            float scaleY = (angle > 90 || angle < -90) ? -originalScaleY : originalScaleY;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);

            if(hasLight){
                Vector3 newRotation = new Vector3(0f, scaleY > 0 ? 180f : 0f, 90f);
                Light.transform.localRotation = Quaternion.Euler(newRotation);
            }
        }

    }

    public void SetupGun(GameObject gunSource, AimController Aim){
        GameObject source = Instantiate(gunSource, Vector3.zero, Quaternion.identity);
        this.Aim = Aim;
        List<Transform> childrenList = new List<Transform>();
        for (int i = 0; i < source.transform.childCount; i++){
            Transform child = source.transform.GetChild(i);
            childrenList.Add(child);
        }

        foreach (Transform child in childrenList)
        {
            if (child != gunSource.transform){
                child.SetParent(this.transform);
                child.transform.localPosition = Vector3.zero;
                child.transform.localScale = new Vector3(1, 1, 1);
                child.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.bulletTarget = this.transform.GetChild(1).GetChild(0);
        Destroy(source);

        targetRefer = transform;

        if(this.Aim && this.Aim.IsReady())
            this.Aim.ActiveMode();

    }

    public PickableComponent PickObject() => this.pickableComponent;
}
