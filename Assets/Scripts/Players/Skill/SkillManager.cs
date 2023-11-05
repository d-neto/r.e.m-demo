using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private List<Skill> Skills = new List<Skill>();

    public void AddSkill(Skill skill){
        Skills.Add(skill);
    }
    public void RemoveSkill(string skillName){
        foreach(Skill skill in Skills)
            if(skill.name == skillName)
                Skills.Remove(skill);
    }

    public Player GetPlayer() => this.player;
}