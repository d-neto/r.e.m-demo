using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractXp : Skill
{
    [SerializeField] float speed = 1f;
    [SerializeField] float range = 1f;
    [SerializeField] LayerMask whatsIsXP;
    void Update()
    {      
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, range, whatsIsXP);
        if(objects != null){
            for(int i = 0; i < objects.Length; i++){
                Vector2 direction = (transform.position - objects[i].transform.position).normalized;
                objects[i].GetComponent<Rigidbody2D>().velocity = direction*speed;
            }
        }
    }

    public void UpdateValue(float r, float s){
        this.range += r;
        this.speed += s;
    }

}
