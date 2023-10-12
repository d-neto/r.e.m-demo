using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public string animationName {get; private set;}

    public virtual void OnEnter(){
        // Do something...
    }

    public virtual void OnExit(){
        // Do something...
    }

    public virtual void OnUpdate(){
        // Do something...
    }

    public virtual void OnFixedUpdate(){
        // Do something...
    }

    public virtual void AnimationEvents(){
        // Do something...
    }

}
