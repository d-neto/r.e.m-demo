using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private List<Skill> Skills = new List<Skill>();

    public Skill FindSkill(string skillName){
        foreach(Skill skill in Skills)
            if(skill.name == skillName)
                return skill;
        return null;
    }
    public bool ContainSkills(List<string> skills){
        bool containAll = true;
        foreach(string skill in skills){
            if(!Skills.Find(s => s.GetName() == skill)){
                containAll = false;
                break;
            }
        }
        return containAll;
    }
    public void AddSkill(Skill skill){
        skill.SetManager(this);
        Skills.Add(skill);
    }
    public void AddSkillByPrefab(GameObject skillPrefab){
        GameObject skillClone = Instantiate(skillPrefab, null);
        skillClone.transform.SetParent(this.transform);
        skillClone.transform.localPosition = Vector3.zero;
        skillClone.SetActive(true);
        Skill skill = skillClone.GetComponent<Skill>();
        skill.SetManager(this);
        Skills.Add(skill);
    }
    public void RemoveSkill(string skillName){
        foreach(Skill skill in Skills)
            if(skill.name == skillName)
                Skills.Remove(skill);
    }
    public void RemoveSkill(Skill skill){
        Skills.Remove(skill);
    }

    public int CountSkillByName(string skillName){
        int count = 0;
        foreach(Skill skill in Skills)
            if(skill.GetName() == skillName)
                count++;
        return count;
    }

    public Player GetPlayer() => this.player;
}