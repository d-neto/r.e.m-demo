using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    [SerializeField] protected string skillName;
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

    public void SetManager(SkillManager manager){
        this.Manager = manager;
    }

    public string GetName() => this.skillName;
}