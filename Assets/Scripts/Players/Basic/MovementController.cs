using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController
{

    [SerializeField] private float speed;

    private Vector2 input = Vector2.zero;
    private Vector2 movement = Vector2.zero;
    private Rigidbody2D rbody;

    public MovementController(Rigidbody2D rbody, float speed){
        this.rbody = rbody;
        this.speed = speed;
    }

    public void Update()
    {
        // input.x = Input.GetAxisRaw("Horizontal");
        // input.y = Input.GetAxisRaw("Vertical");
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        // movement = input.normalized;
        movement = input;
        rbody.velocity = movement * speed;
    }

    public bool IsMoving(){
        return (Mathf.Abs(rbody.velocity.x) > 0.1 || Mathf.Abs(rbody.velocity.y) > 0.1)
            && (Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0);
    }

}
