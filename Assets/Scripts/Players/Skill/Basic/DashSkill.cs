using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill {

    [Header("Configs.")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTimer;
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] GameObject ghostPrefab;

    public override void OnUpdate()
    {
        if(Input.GetButtonDown("Dash") && canDash && Manager.GetPlayer().Movement.IsMoving()) Dash();

        if(isDashing){
            GameObject ghost = Instantiate(ghostPrefab, Manager.GetPlayer().transform.position, Manager.GetPlayer().transform.rotation);
            ghost.transform.parent = null;
            ghost.transform.localScale = Manager.GetPlayer().transform.localScale;
            ghost.GetComponent<SpriteRenderer>().sprite = Manager.GetPlayer().Graphics().sprite;
        }
    }

    void Dash(){
        this.canDash = false;
        this.isDashing = true;
        this.Manager.GetPlayer().DisableCollision();
        this.Manager.GetPlayer().Movement.Rbody().AddForce(dashSpeed * this.Manager.GetPlayer().Movement.GetRaw(), ForceMode2D.Impulse);
        this.Manager.GetPlayer().Movement.Lock(true);
        base.StartCoroutine(DisableDash());
    }

    IEnumerator DisableDash(){
        yield return new WaitForSeconds(dashTimer);
        canDash = true;
        isDashing = false;
        this.Manager.GetPlayer().Movement.Lock(false);
        this.Manager.GetPlayer().Movement.Rbody().velocity = Vector2.zero;
        this.Manager.GetPlayer().Invoke("EnableCollision", 0.5f);
    }
}