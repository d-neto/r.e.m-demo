using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDamageState : PlayerState
{

    private Transform origin;
    public BasicDamageState(string animationName, Player player, PlayerStateMachine stateMachine, PlayerData playerData)
    : base(animationName, player, stateMachine, playerData){}
    public override void Enter()
    {
        this.DoChecks();
        player.GetAnimator().SetTrigger(this.animationName);
        this.startTime = Time.time;

        Vector2 direction = (player.transform.position - origin.position).normalized;
        player.Movement.Rbody().AddForce(direction * 5, ForceMode2D.Impulse);
        player.Movement.Lock(true);
        player.PlayAudio(player.Data.ACHurt, 0.5f);
        player.StartCoroutine(Damaged());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void AnimationEvents()
    {
        base.AnimationEvents();
    }

    IEnumerator Damaged(){
        player.DisableCollision();
        stateMachine.ChangeState(player.States.IdleState);

        player.Graphics().color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);

        player.Movement.Lock(false);

        player.Graphics().color = new Color(255, 255, 255, 0.7f);
        yield return new WaitForSeconds(0.15f);
        player.Graphics().color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);
        player.Graphics().color = new Color(255, 255, 255, 0.7f);
        yield return new WaitForSeconds(0.15f);
        player.Graphics().color = new Color(255, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.15f);
        player.Graphics().color = new Color(255, 255, 255, 1f);

        yield return new WaitForSeconds(0.5f);
        player.EnableCollision();
    }

    public void SetOrigin(Transform origin) => this.origin = origin;
}
