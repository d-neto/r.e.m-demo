using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{

    public Player withPlayer;

    [Header("Configs.")]
    [SerializeField] private float maxAngle = 360.0f;
    [SerializeField] private Transform targetRefer;
    [SerializeField] private bool canInvertOn90Degree = true;

    [Header("Attributes")]
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int loadedAmmo;

    [SerializeField] private int fireRateInMS;
    [SerializeField] private int reloadTimeInMS;
    protected float reloadTime;
    protected float fireRate;

    Vector3 mousePosition;
    Vector3 positionRefer;
    Vector3 mouseDirection;
    float angle;
    float originalScaleY;

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

        mousePosition = Input.mousePosition;
        positionRefer = Camera.main.WorldToScreenPoint(targetRefer.position);
        mouseDirection = mousePosition - positionRefer;

        angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(canInvertOn90Degree){
            float scaleY = (angle > 90 || angle < -90) ? -originalScaleY : originalScaleY;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
        }

    }

    public void SetupGun(GameObject gunSource){
        // GetChild(0) -> [GFX]
        // GetChild(1) -> [Positions]
        GameObject source = Instantiate(gunSource, Vector3.zero, Quaternion.identity);
        source.transform.SetParent(this.transform.GetChild(0));

        // GetChild(0) om source object must be the position of bullet spawn
        source.transform.GetChild(0).SetParent(this.transform.GetChild(1));
    }

}
