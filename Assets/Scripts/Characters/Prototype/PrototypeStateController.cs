using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeStateController : PlayerStateController
{
    MovementController movement;
    public override void StateController(){
        if(movement.IsMoving()) ChangeState(Running);
    }

}
