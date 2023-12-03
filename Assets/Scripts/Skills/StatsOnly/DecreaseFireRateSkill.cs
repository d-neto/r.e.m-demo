using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseFireRateSkill : Skill
{

    [SerializeField] float value;
    public override void OnStart()
    {
        base.OnStart();
        foreach(Gun g in Manager.GetPlayer().Config.GetAllGuns())
            g.LessFireRate(value);

        Manager.RemoveSkill(this);
        Destroy(this.gameObject);
    }

}
