using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AIM_MODE{Mouse, Target, Joystick, JoystickAuto};
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
        ActiveMode();
    }

    void Start(){
        Cursor.visible = false;

        player.GetInput().OnInputFire += FireAnimation;
        player.GetInput().OnReloadStart += ReloadStart;
        player.GetInput().OnReloadEnd += ReloadEnd;
    }

    void OnEnable() {
        Cursor.visible = false;
    }

    bool disabled = false;
    bool isLocked = false;
    void Update(){
        if(isLocked) return;
        if(player.Config.HasGuns())
            TargetModeUpdate();

        if(currentTiming <= 0) canFireAnimation = true;
        if(currentTiming >= 0) currentTiming -= Time.deltaTime;
    }

    public void UseMouse(){
        if(disabled){
            Disabled(false);
        }
        mousePosition = Input.mousePosition;
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(mousePosition);
        SetTarget(transform);
    }
    public void UseTarget(){
        if(disabled){
            Disabled(false);
        }

        NewSearchTarget();
        
        if(target && Vector2.Distance(player.transform.position, target.position) > 15){
            SwitchTarget();
        }

        if(player.GetInput().GetSwitchTargetDown()) SwitchTarget();
        transform.position = target ? target.position : transform.position;
    }

    public void UseJoystick(){
        if(disabled){
            Disabled(false);
        }
        if(player.GetInput().GetAxisAnalog() == Vector2.zero){
            transform.position = nullTarget.position;
            this.target = transform;
            NoTarget();
        }else{
            transform.position = (Vector2) player.transform.position +(player.GetInput().GetAxisAnalog()*3f);
            SetTarget(transform);
        }
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
    public void Disabled(bool value){
        disabled = value;
        this.Anim.SetBool("disabled", value);
    }

    public void ActiveMode(){
        switch(mode){
            case AIM_MODE.Target:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithActualTarget);
                ChangeMode(UseTarget);
                break;
            case AIM_MODE.Mouse:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithMouse);
                ChangeMode(UseMouse);
                break;
            case AIM_MODE.Joystick:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithActualTarget);
                ChangeMode(UseJoystick);
                break;
            case AIM_MODE.JoystickAuto:
                this.player.Movement.ChangeInvertMode(this.player.Movement.InvertWithActualTarget);
                ChangeMode(UseJoystick);
                break;
        }
    }

    List<AbleAim> targets = new List<AbleAim>();

    [SerializeField]
    AbleAim actualTarget;
    int targetIndex;
    List<AbleAim> tempTargets;
    public void NewSearchTarget(float range = 15, bool findFirst = false){
        if(!EnemyManager.Instance){
            if(!disabled){
                SetNullTarget();
                targets.RemoveAll((e) => true);
                player.Movement.SetNullTarget(true);
                actualTarget = null;
                Disabled(true);
            }
            return;
        }
        tempTargets = EnemyManager.Instance.GetEnemiesInRadius(player.transform.position, range);
        if(tempTargets.Count == 0){
            if(!disabled){
                SetNullTarget();
                targets.RemoveAll((e) => true);
                player.Movement.SetNullTarget(true);
                actualTarget = null;
                Disabled(true);
            }
            return;
        }

        AbleAim choose = actualTarget;
        if(!actualTarget){
            choose = tempTargets.First();
            targetIndex = 0;
            SetTarget(choose.Get());
        }

        float distFromPlayer, distFromChoose, distChooseFromOther;
        int idx = 0;
        foreach(AbleAim target in tempTargets){

            if(targets != null && targets.Contains(target)) continue;
            else targets.Add(target);

            distFromPlayer = Vector2.Distance(transform.position, target.Get().position);
            distFromChoose = Vector2.Distance(transform.position, choose.Get().position);
            distChooseFromOther = Vector2.Distance(target.Get().position, choose.Get().position);

            if(distFromPlayer < distFromChoose && distChooseFromOther > 2){
                choose = target;
                targetIndex = idx;
            }

            idx++;
        }

        for(int i = 0; i < targets.Count; i++){
            if(!tempTargets.Contains(targets[i])) targets.Remove(targets[i]);
        }

        if(!actualTarget || !findFirst)
            actualTarget = choose;

        if(player.Movement.IsNullTarget()){
            player.Movement.SetNullTarget(false);
        }

        if(disabled){
            Disabled(false);
        }
    }

    public void SwitchTarget(){
        if(targets.Count == 0) return;
        targetIndex++;
        if(targetIndex >= targets.Count) targetIndex = 0;
        this.actualTarget = targets[targetIndex];
        SetTarget(actualTarget.Get());
    }

    public bool IsReady() => this.player.Movement != null;
    void SetNullTarget() => this.target = nullTarget;
    public void Setup(Player player, Transform nullTarget){
        this.player = player;
        this.nullTarget = nullTarget;
    }
    public void Setup(Player player, Transform nullTarget, AIM_MODE mode){
        Setup(player, nullTarget);
        this.mode = mode;
    }

    public void Lock(bool status) => this.isLocked = status;
}
