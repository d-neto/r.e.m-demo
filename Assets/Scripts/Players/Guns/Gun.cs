using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{

    public delegate void RotationMethod();
    public RotationMethod ApplyRotation;
    public Player withPlayer;

    [Header("Configs.")]
    [SerializeField] private string targetMethod = "mouse";
    [SerializeField] private AbleAim actualTarget;
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

        if(targetMethod == "target") ChangeRotationToTarget();
        else ChangeRotationToMouse();
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


    public void ChangeRotationToTarget(){
        ChangeRotationMethod(RotationByTarget);
        this.withPlayer.Movement.ChangeInvertMode(this.withPlayer.Movement.InvertWithActualTarget);
        Aim.ChangeMode(Aim.UseTarget);
    }
    public void ChangeRotationToMouse(){
        ChangeRotationMethod(RotationByMouse);
        this.withPlayer.Movement.ChangeInvertMode(this.withPlayer.Movement.InvertWithMouse);
        Aim.ChangeMode(Aim.UseMouse);
    }

    public void ChangeRotationMethod(RotationMethod method){
        this.ApplyRotation = method;
    }

    public virtual void RotationByMouse(){

        BulletToDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        positionRefer = Camera.main.WorldToScreenPoint(targetRefer.position);
        mouseDirection = Aim.GetByMouse() - positionRefer;

        angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(canInvertOn90Degree){
            float scaleY = (angle > 90 || angle < -90) ? -originalScaleY : originalScaleY;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
        }

    }
    public virtual void RotationByTarget(){
        if(!actualTarget){
            BulletToDirection = transform.position + bulletTarget.right;
            this.withPlayer.Movement.RemoveActualTarget();
            this.Aim.NoTarget();
            return;
        }

        BulletToDirection = actualTarget.Get().position;
        this.withPlayer.Movement.SerActualTarget(actualTarget.Get());
        this.Aim.SetTarget(actualTarget.Get());

        rotateToTargetRefer = Camera.main.WorldToScreenPoint(actualTarget.Get().position);
        positionRefer = Camera.main.WorldToScreenPoint(targetRefer.position);
        mouseDirection = rotateToTargetRefer - positionRefer;

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

    AbleAim[] targets;
    int targetIndex;
    bool isRotationDefault = false;
    public void SearchTarget(bool findFirst = false){
        targets = EnemyManager.Instance.GetEnemiesInRadius(withPlayer.transform.position, 15);
        if(targets.Length == 0){
            if(!isRotationDefault){
                transform.localRotation = Quaternion.AngleAxis(0f, new Vector3(0, 0, 0));
                transform.localScale = new Vector3(1, 1, 1);
                isRotationDefault = true;
            }
            return;
        }

        AbleAim choose = actualTarget;
        if(!actualTarget){
            choose = targets[0];
            targetIndex = 0;
        }

        float distFromPlayer, distFromChoose;

        int idx = 0;
        foreach(AbleAim target in targets){
            distFromPlayer = Vector2.Distance(transform.position, target.transform.position);
            distFromChoose = Vector2.Distance(transform.position, choose.transform.position);
            if(distFromPlayer < distFromChoose && distFromChoose > 2){
                choose = target;
                targetIndex = idx;
            }
            idx++;
        }
        if(!actualTarget || !findFirst)
            actualTarget = choose;

        isRotationDefault = false;
    }

    public void SwitchTarget(){
        this.actualTarget = targets[targetIndex+1 >= targets.Length ? 0 : ++targetIndex];
    }

}
