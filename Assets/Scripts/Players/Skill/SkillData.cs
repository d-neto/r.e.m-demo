using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newSkillSet", menuName = "Skill/ New skill set")]
public class SkillData : ScriptableObject {
    public List<SkillOption> skills;
}