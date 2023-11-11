using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGroup
{

    public PlayerState IdleState;
    public PlayerState MoveState;
    public PlayerState DamageState;
    public PlayerState DeadState;

    public StateGroup(Player player, PlayerStateMachine stateMachine, PlayerData data){
        this.IdleState = new BasicIdleState("idle", player, stateMachine, data);
        this.MoveState = new BasicMoveState("running", player, stateMachine, data);
        this.DamageState = new BasicDamageState("damage", player, stateMachine, data);
        this.DeadState = new BasicDeadState("dead", player, stateMachine, data);
    }

}
