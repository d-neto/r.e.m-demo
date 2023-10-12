using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] private float speed;

    private Vector2 input = Vector2.zero;
    private Vector2 movement = Vector2.zero;
    private Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        movement = input.normalized;
        rbody.velocity = movement * speed;
    }

    public bool IsMoving(){
        return (Mathf.Abs(rbody.velocity.x) > 0.1 || Mathf.Abs(rbody.velocity.y) > 0.1)
            && (Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0);
    }

}
