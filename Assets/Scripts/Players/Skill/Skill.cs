using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    protected string skillName;
    [SerializeField] protected SkillManager Manager;

    void Start(){
        this.OnStart();
    }
    void Update(){
        this.OnUpdate();
    }

    public virtual void OnUpdate(){
        //Do Something...
    }
    public virtual void OnStart(){
        //Do Something...
    }

}