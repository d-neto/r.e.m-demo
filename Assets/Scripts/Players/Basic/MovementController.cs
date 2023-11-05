using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController
{

    [SerializeField] private float speed;

    private Vector2 input = Vector2.zero;
    private Vector2 movement = Vector2.zero;
    private Vector2 movementRaw = Vector2.zero;
    private Rigidbody2D rbody;
    private Player player;

    bool canMove = true;

    public MovementController(Player player){
        this.player = player;
        this.rbody = player.GetComponent<Rigidbody2D>();
        this.speed = player.Data.speed;
    }

    public void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        movement = input;
        movementRaw.x = Input.GetAxisRaw("Horizontal");
        movementRaw.y = Input.GetAxisRaw("Vertical");

        if(canMove) Movement();
    }

    public void Movement(){
        rbody.velocity = movement * speed;
        InvertWithMouse();
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

    public Vector2 Get() => this.movement;
    public Vector2 GetRaw() => this.movementRaw;
    public Rigidbody2D Rbody() => this.rbody;

    public void Lock(bool isLocked) => this.canMove = !isLocked;
}
