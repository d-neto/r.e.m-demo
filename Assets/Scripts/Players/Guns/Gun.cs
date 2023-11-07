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
    [SerializeField] private bool canInvertOn90Degree = true;

    [Header("Audios")]
    [SerializeField] protected AudioClip shootAudioClip;
    [SerializeField] protected AudioClip reloadAudioClip;
    [SerializeField] protected AudioClip emptyAudioClip;

    [Header("Attributes")]
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int loadedAmmo;

    [SerializeField] private int fireRateInMS;
    [SerializeField] private int reloadTimeInMS;
    [SerializeField] protected Transform bulletTarget;
    [SerializeField] protected AimController Aim;

    protected float reloadTime;
    protected float fireRate;

    Vector3 positionRefer;
    Vector3 rotateToTargetRefer;
    Vector3 mouseDirection;
    protected Vector2 BulletToDirection;
    float angle;
    float originalScaleY;

    private void Awake(){
        if(!this.Audio)
            this.Audio = GetComponent<AudioSource>();
    }

    private void Start(){
        reloadTime = (float) reloadTimeInMS / 1000f;
        fireRate = (float) fireRateInMS / 1000f;

        originalScaleY = transform.localScale.y;

        if(targetRefer == null) targetRefer = transform;

        OnStart();
    }

    private void OnEnable() => OnEnableGun();

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
        if(withPlayer == null) withPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void ApplyRotation(){

        if(!Aim.GetActualTarget()){
            return;
        }

        BulletToDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        positionRefer = Camera.main.WorldToScreenPoint(targetRefer.position);
        mouseDirection =  Aim.GetScreenPosition() - positionRefer;

        this.withPlayer.Movement.SerActualTarget(Aim.GetActualTarget());
        this.BulletToDirection = Aim.GetActualTarget().position;

        angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);


        if(canInvertOn90Degree){
            float scaleY = (angle > 90 || angle < -90) ? -originalScaleY : originalScaleY;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
        }

    }

    public void SetupGun(GameObject gunSource){
        GameObject source = Instantiate(gunSource, Vector3.zero, Quaternion.identity);

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
            }
        }
        Destroy(source);
    }
}
