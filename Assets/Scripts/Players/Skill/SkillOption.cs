using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillOption{
    public string name;
    public int minLevel;
    public int maxLevel;
    public int maxCount;
    public List<int> onlyForLevels;
    public List<int> isNotForLevels;
    public List<string> dependencies;
    public GameObject graphics;
    public GameObject prefab;

    public SkillOption Clone(){
        SkillOption clone = new SkillOption();
        clone.graphics = graphics;
        clone.prefab = prefab;
        clone.maxLevel = maxLevel;
        clone.minLevel = minLevel;
        clone.isNotForLevels = new List<int>(isNotForLevels);
        clone.onlyForLevels = new List<int>(onlyForLevels);
        return clone;
    }

}