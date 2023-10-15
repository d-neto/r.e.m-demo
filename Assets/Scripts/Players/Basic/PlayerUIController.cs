using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Slider skillPowerSlider;

    [Header("Gun")]
    [SerializeField] private SpriteRenderer gunIcon;
    [SerializeField] private Text ammoText;


    public Slider LifeSlider() => this.lifeSlider;
    public Slider SkillSlider() => this.skillPowerSlider;
}
