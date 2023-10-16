using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    private string animationName;
    protected float startTime;

    public PlayerState(string animationName, Player player, PlayerStateMachine stateMachine, PlayerData playerData){
        this.animationName = animationName;
        this.player = player;
        this.playerData = playerData;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter(){
        this.DoChecks();
        player.GetAnimator().SetBool(this.animationName, true);
        this.startTime = Time.time;
        // Do something...
    }

    public virtual void Exit(){
        player.GetAnimator().SetBool(this.animationName, false);
    }

    public virtual void Update(){
        // Do something...
    }

    public virtual void FixedUpdate(){
        // Do something...
    }

    public virtual void DoChecks(){
        // Do something...
    }

    public virtual void AnimationEvents(){
        // Do something...
    }

}
