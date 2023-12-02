using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private GameObject HUD;
    [SerializeField] private Animator lifeSliderAnimator;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Slider skillPowerSlider;
    [SerializeField] private Slider XPSlider;
    [SerializeField] private TMP_Text levelText;

    [Header("Gun")]
    [SerializeField] private SpriteRenderer gunIcon;
    [SerializeField] private Text ammoText;

    void Awake(){
        HUD = Instantiate(HUD, null);
        for(int i = 0; i < HUD.transform.childCount; i++){
            if(HUD.transform.GetChild(i).CompareTag("HUD:HealthSlider")){
                this.lifeSlider = HUD.transform.GetChild(i).GetComponent<Slider>();
                this.lifeSliderAnimator = HUD.transform.GetChild(i).GetComponent<Animator>();
            }
            if(HUD.transform.GetChild(i).CompareTag("HUD:XPSlider"))
                this.XPSlider = HUD.transform.GetChild(i).GetComponent<Slider>();
            if(HUD.transform.GetChild(i).CompareTag("HUD:Level"))
                this.levelText = HUD.transform.GetChild(i).GetComponent<TMP_Text>();
        }
    }

    void Start(){
        GameObject[] huds = GameObject.FindGameObjectsWithTag("UI:HUDPosition");
        for(int i = 0; i < huds.Length; i++){
            if(huds[i].transform.childCount == 0){
                this.HUD.transform.SetParent(huds[i].transform, false);
            }
        }
        Player player = this.GetComponent<Player>();
        player.Stats.VerifyXP();
        player.Stats.UpdateLifeSlider();
    }


    public Slider LifeSlider() => this.lifeSlider;
    public Slider ExpSlider() => this.XPSlider;
    public Slider SkillSlider() => this.skillPowerSlider;
    public TMP_Text LevelText() => this.levelText;
    public Animator LifeSliderAnimator() => this.lifeSliderAnimator;
}
