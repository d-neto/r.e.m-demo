using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public delegate void TargetMode();
    TargetMode TargetModeUpdate;

    [Header("Colors")]
    [SerializeField] private Color normalAimColor;
    [SerializeField] private Color invalidAimColor;
    [SerializeField] private Color targetAimColor;

    [Header("Configs.")]
    [SerializeField] private List<SpriteRenderer> sprites;

    [SerializeField] private Animator Anim;

    Vector3 mousePosition;
    Transform target;
    float timingShoot = 0.3f;
    float currentTiming = 0.3f;
    bool canFireAnimation = true;

    void Awake(){
        this.TargetModeUpdate = UseMouse;
        SetColor(normalAimColor);
        if(!Anim) Anim = GetComponent<Animator>();
    }

    void Start(){
        Cursor.visible = false;

        InputHandler.OnInputFire += FireAnimation;
        InputHandler.OnReloadStart += ReloadStart;
        InputHandler.OnReloadEnd += ReloadEnd;
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
    }
    public void UseTarget(){
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
        InputHandler.OnInputFire -= FireAnimation;
        InputHandler.OnReloadStart -= ReloadStart;
        InputHandler.OnReloadEnd -= ReloadEnd;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")) SetColor(targetAimColor);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")) SetColor(normalAimColor);
    }

    public Vector3 GetByMouse() => this.mousePosition;
    public void SetTarget(Transform target){
        this.Anim.SetBool("no-target", false);
        this.target = target;
    }
    public void ChangeMode(TargetMode mode) => this.TargetModeUpdate = mode;
    public void NoTarget() => this.Anim.SetBool("no-target", true);
}
