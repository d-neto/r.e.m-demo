using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerStateController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public delegate void StateReference();
    protected StateReference CurrentState;

    protected bool canChangeState = true;
    void Start(){
        CurrentState = Idle;
    }

    void Update(){
        StateController();
        CurrentState();
    }

    public virtual void StateController(){}
    public virtual void Idle(){}
    public virtual void Running(){}
    public virtual void Dashing(){}
    public virtual void Attacking(){}
    public virtual void Shooting(){}
    public virtual void Dead(){}

    public void ChangeState(StateReference state, bool force = false){
        if(canChangeState)
            CurrentState = state;
        if(force) CurrentState = state;
    }

}
