using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseXpAttractor : Skill
{

    [SerializeField] float speed;
    [SerializeField] float range;
    public override void OnStart()
    {
        base.OnStart();
        AttractXp skill = (AttractXp) Manager.FindSkill("XPAttractor");
        skill.UpdateValue(range, speed);
        Manager.RemoveSkill(this);
        Destroy(this.gameObject);
    }

}
