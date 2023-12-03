using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : Skill
{

    [SerializeField] float damage;
    public override void OnStart()
    {
        base.OnStart();
        foreach(Gun g in Manager.GetPlayer().Config.GetAllGuns())
            g.AddDamage(damage);

        Manager.RemoveSkill(this);
        Destroy(this.gameObject);
    }

}
