using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController
{

    public delegate void InvertSpriteMode();
    private InvertSpriteMode InvertWith;
    [SerializeField] private float speed;

    private Vector2 input = Vector2.zero;
    private Vector2 movement = Vector2.zero;
    private Vector2 movementRaw = Vector2.zero;
    private Rigidbody2D rbody;
    private Player player;

    bool canMove = true;

    Transform target;

    public MovementController(Player player){
        this.player = player;
        this.rbody = player.GetComponent<Rigidbody2D>();
        this.speed = player.Data.speed;

        this.InvertWith = InvertWithMouse;
    }

    public void Update()
    {
        input.x = player.GetInput().GetHorizontal();
        input.y =  player.GetInput().GetVertical();
        movement = input;
        movementRaw.x =  player.GetInput().GetHorizontalRaw();
        movementRaw.y =  player.GetInput().GetVerticalRaw();

        if(canMove) Movement();
    }

    public void Movement(){
        rbody.velocity = movement * speed;
        InvertWith();
    }

    public bool IsMoving(){
        return (Mathf.Abs(rbody.velocity.x) > 0.1 || Mathf.Abs(rbody.velocity.y) > 0.1)
            && (Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0);
    }

    public void InvertWithMouse(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (player.transform.position - mousePosition).normalized;

        if(direction.x > 0) player.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if(direction.x < 0) player.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    bool nullTarget = false;
    public void InvertWithActualTarget(){
        Vector3 direction = -input;
        if(!nullTarget && target){
            direction = (player.transform.position - target.position).normalized;
        }

        if(direction.x > 0) player.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if(direction.x < 0) player.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public Vector2 Get() => this.movement;
    public Vector2 GetRaw() => this.movementRaw;
    public Rigidbody2D Rbody() => this.rbody;

    public void Lock(bool isLocked) => this.canMove = !isLocked;
    public void ChangeInvertMode(InvertSpriteMode mode) => this.InvertWith = mode;
    public void SerActualTarget(Transform target) => this.target = target;
    public void RemoveActualTarget() => this.target = null;
    public void SetNullTarget(bool nullTarget) => this.nullTarget = nullTarget;
    public bool IsNullTarget() => this.nullTarget;

    public bool CanMove() => this.canMove;
}
