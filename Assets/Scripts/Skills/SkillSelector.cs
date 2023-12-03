using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    
    public static SkillSelector Instance;
    public GameObject SkillCanvas;
    public SkillData Data;
    public List<SkillOption> AllSkills;
    public List<Transform> skillPositions;
    public AudioSource audioSource;
    public AudioClip confirmAudioClip;
    public AudioClip selectionAudio;

    Player actualPlayer;
    int actualIndex = 0;
    bool isChoosing = false;
    bool pressedAxis = false;
    [SerializeField] List<SkillOption> actualSkills = new List<SkillOption>();

    void Awake(){
        if(!Instance) Instance = this;
        else Destroy(this);

        Data = Instantiate<SkillData>(Data);
        AllSkills = Data.skills;
        if(!this.audioSource) audioSource = GetComponent<AudioSource>();
    }

    void Update(){

        if(!isChoosing || !actualPlayer) return;
        
        if(actualPlayer.GetInput().GetAnalogMovementRaw().x > 0 && !pressedAxis){
            actualSkills[actualIndex].graphics.transform.GetChild(0).gameObject.SetActive(false);
            actualIndex++;
            pressedAxis = true;
            audioSource.PlayOneShot(selectionAudio, 0.1f);
        }else if(actualPlayer.GetInput().GetAnalogMovementRaw().x < 0 && !pressedAxis){
            actualSkills[actualIndex].graphics.transform.GetChild(0).gameObject.SetActive(false);
            actualIndex--;
            pressedAxis = true;
            audioSource.PlayOneShot(selectionAudio, 0.1f);
        }else if(actualPlayer.GetInput().GetAnalogMovementRaw().x == 0) pressedAxis = false;

        if(actualIndex < 0) actualIndex = 0;
        if(actualIndex >= actualSkills.Count) actualIndex = actualSkills.Count-1;
        actualSkills[actualIndex].graphics.transform.GetChild(0).gameObject.SetActive(true);

        if(actualPlayer.GetInput().GetConfirmDown()){
            actualPlayer.GetSkillManager().AddSkillByPrefab(actualSkills[actualIndex].prefab);
            actualPlayer = null;
            actualIndex = 0;
            actualSkills.Clear();
            isChoosing = false;
            SkillCanvas.SetActive(false);
            pressedAxis = false;
            ClearPositions();
            audioSource.PlayOneShot(confirmAudioClip, 0.3f);
            Time.timeScale = 1f;
        }
    }

    public void StartSelector(Player player){
        this.actualPlayer = player;
        actualSkills.Clear();
        this.actualIndex = 0;

        List<SkillOption> playerCanReceive = new List<SkillOption>();
        for(int i = 0; i < AllSkills.Count; i++){
            if(actualPlayer.Data.level < AllSkills[i].minLevel) continue;
            if(actualPlayer.Data.level > AllSkills[i].maxLevel && AllSkills[i].maxLevel != 0) continue;
            if(AllSkills[i].isNotForLevels.Count > 0 && AllSkills[i].isNotForLevels.Contains(actualPlayer.Data.level)) continue;
            if(AllSkills[i].onlyForLevels.Count > 0 && !AllSkills[i].isNotForLevels.Contains(actualPlayer.Data.level)) continue;
            if(AllSkills[i].dependencies.Count > 0 && !actualPlayer.GetSkillManager().ContainSkills(AllSkills[i].dependencies)) continue;
            if(AllSkills[i].maxCount > 0 && actualPlayer.GetSkillManager().CountSkillByName(AllSkills[i].name) >= AllSkills[i].maxCount) continue;
            
            playerCanReceive.Add(AllSkills[i].Clone());
        }

        for(int i = 0; i < skillPositions.Count && playerCanReceive.Count > 0; i++){
            SkillOption newSkill = playerCanReceive[Random.Range(0, playerCanReceive.Count)];
            if(!actualSkills.Contains(newSkill)){
                actualSkills.Add(newSkill);
                playerCanReceive.Remove(newSkill);
            }
        }

        for(int i = 0; i < skillPositions.Count && i < actualSkills.Count; i++){
            GameObject clone = Instantiate(actualSkills[i].graphics, skillPositions[i]);
            clone.transform.GetChild(0).gameObject.SetActive(false);
            actualSkills[i].graphics = clone;
        }

        SkillCanvas.SetActive(true);
        isChoosing = true;
        Debug.Log("ISCHOOSING: "+isChoosing);
        Debug.Log("PLAYER: "+actualPlayer);
        Time.timeScale = 0f;
    }

    void ClearPositions(){
        foreach(Transform t in skillPositions){
            for(int i = 0; i < t.childCount; i++)
                Destroy(t.GetChild(i).gameObject);
        }
    }

}
