using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveState : PlayerState
{

    public BasicMoveState(string animationName, Player player, PlayerStateMachine stateMachine, PlayerData playerData)
    : base(animationName, player, stateMachine, playerData){}
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(!player.Movement.IsMoving()) stateMachine.ChangeState(player.States.IdleState);
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

}
