using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{   
    [SerializeField] protected Player player;
    [SerializeField] protected Transform nullTarget;
    public delegate void TargetMode();
    TargetMode TargetModeUpdate;

    [Header("Colors")]
    [SerializeField] private Color normalAimColor;
    [SerializeField] private Color invalidAimColor;
    [SerializeField] private Color targetAimColor;

    public enum AIM_MODE{Mouse, Target};
    [Header("Configs.")]
    [SerializeField] public AIM_MODE mode = AIM_MODE.Mouse;
    [SerializeField] private List<SpriteRenderer> sprites;

    [SerializeField] private Animator Anim;

    Vector3 mousePosition;
    Transform target;
    float timingShoot = 0.3f;
    float currentTiming = 0.3f;
    bool canFireAnimation = true;

    void Awake(){
        SetColor(normalAimColor);
        if(!Anim) Anim = GetComponent<Animator>();
    }

    void Start(){
        Cursor.visible = false;

        player.GetInput().OnInputFire += FireAnimation;
        player.GetInput().OnReloadStart += ReloadStart;
        player.GetInput().OnReloadEnd += ReloadEnd;

        switch(mode){
            case AIM_MODE.Target:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithActualTarget);
                ChangeMode(UseTarget);
                break;
            case AIM_MODE.Mouse:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithMouse);
                ChangeMode(UseMouse);
                break;
        }
    }

    void OnEnable() {
        Cursor.visible = false;
    }

    void Update(){
        
        TargetModeUpdate();

        if(currentTiming <= 0) canFireAnimation = true;
        if(currentTiming >= 0) currentTiming -= Time.deltaTime;
    }

    public void UseMouse(){
        mousePosition = Input.mousePosition;
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(mousePosition);
        SetTarget(transform);
    }
    public void UseTarget(){
        SearchTarget();
        if(player.GetInput().GetSwitchTarget()) SwitchTarget();
        transform.position = target ? target.position : transform.position;
    }

    void SetColor(Color color){
        foreach(SpriteRenderer sprite in sprites){
            sprite.color = color;
        }
    }

    void FireAnimation(){
        if(!Anim.GetBool("reloading") && canFireAnimation){
            Anim.SetTrigger("fire");
            canFireAnimation = false;
            currentTiming = timingShoot;
        }
    }
    void ReloadStart(Gun gun){
        Anim.SetTrigger("reload");
        Anim.SetBool("reloading", true);
    }
    void ReloadEnd(Gun gun){
        Anim.SetBool("reloading", false);
    }

    private void OnDestroy() {
        player.GetInput().OnInputFire -= FireAnimation;
        player.GetInput().OnReloadStart -= ReloadStart;
        player.GetInput().OnReloadEnd -= ReloadEnd;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")) SetColor(targetAimColor);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")) SetColor(normalAimColor);
    }

    public Vector3 GetByMouse() => this.mousePosition;
    public Vector3 GetScreenPosition() => Camera.main.WorldToScreenPoint(this.transform.position);
    public Transform GetActualTarget() => this.target;
    public void SetTarget(Transform target){
        this.Anim.SetBool("no-target", false);
        this.target = target;
    }
    public void ChangeMode(TargetMode mode) => this.TargetModeUpdate = mode;
    public void NoTarget() => this.Anim.SetBool("no-target", true);




    AbleAim[] targets;
    AbleAim actualTarget;
    int targetIndex;
    bool showNoTarget = false;
    public void SearchTarget(float range = 15, bool findFirst = false){
        targets = EnemyManager.Instance.GetEnemiesInRadius(player.transform.position, range);

        if(targets.Length == 0){
            if(!showNoTarget){
                NoTarget();
                player.Movement.SetNullTarget(true);
                this.target = nullTarget;
                showNoTarget = true;
            }
            return;
        }

        AbleAim choose = actualTarget;
        if(!actualTarget){
            choose = targets[0];
            targetIndex = 0;
        }

        float distFromPlayer, distFromChoose, distChooseFromOther;

        int idx = 0;
        foreach(AbleAim target in targets){
            distFromPlayer = Vector2.Distance(transform.position, target.Get().position);
            distFromChoose = Vector2.Distance(transform.position, choose.Get().position);
            distChooseFromOther = Vector2.Distance(target.Get().position, choose.Get().position);
            if(distFromPlayer < distFromChoose && distChooseFromOther > 2){
                choose = target;
                targetIndex = idx;
            }
            idx++;
        }
        if(!actualTarget || !findFirst)
            actualTarget = choose;

        showNoTarget = false;
        if(player.Movement.IsNullTarget()) player.Movement.SetNullTarget(false);
        SetTarget(actualTarget.Get());
    }

    public void SwitchTarget(){
        if(targets.Length == 0) return;
        targetIndex++;
        if(targetIndex >= targets.Length) targetIndex = 0;
        this.actualTarget = targets[targetIndex];
        SetTarget(actualTarget.Get());
        Debug.Log("SWITCH: " + targetIndex);
    }

}
