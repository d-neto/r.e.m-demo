using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDeadState : PlayerState
{

    public BasicDeadState(string animationName, Player player, PlayerStateMachine stateMachine, PlayerData playerData)
    : base(animationName, player, stateMachine, playerData){}
    public override void Enter()
    {
        base.Enter();
        player.Movement.Lock(true);
        player.DisableCollision();
        player.Movement.Rbody().velocity = Vector2.zero;
        GameObject.Instantiate(player.Data.PSDeathPrefab, player.transform.position, Quaternion.identity);
        player.Graphics().enabled = false;
        for(int i = 0; i < player.transform.childCount; i++)
            player.transform.GetChild(i).gameObject.SetActive(false);
        player.PlayAudio(player.Data.ACDead, 0.8f);

        JoystickVibration.Instance.Rumble(player.GetInput().Get().joystickIndex, 0.05f, 1f, 0.5f);
        PlayerManager.Instance.ShowGameOver();
    }

    public override void Update()
    {
        base.Update();
        if(player.Movement.CanMove())
            player.Movement.Lock(true);
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
