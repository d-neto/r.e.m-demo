using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOverController : MonoBehaviour
{

    public GameObject arrow;
    public List<TMP_Text> options;
    public int startIndex = 1;
    public int selectedOption = 0;
    public int maxOptions = 2;

    public Vector3 newArrowPosition = new Vector3();
    public Color selectedColor;
    public Color normalColor;
    bool pressed = false;

    Player mainPlayer;

    void Start(){
        mainPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
        ChangeOption();
        Invoke(nameof(StopTime), 2f);
    }
    void Update()
    {
        if(mainPlayer.GetInput().GetVerticalRaw() < 0 && !pressed){
            pressed = true;
            options[selectedOption].color = normalColor;
            selectedOption++;
            ChangeOption();
        }

        if(mainPlayer.GetInput().GetVerticalRaw() > 0 && !pressed){
            pressed = true;
            options[selectedOption].color = normalColor;
            selectedOption--;
            ChangeOption();
        }

        if(mainPlayer.GetInput().GetVerticalRaw() == 0) pressed = false;

        newArrowPosition.x = arrow.transform.localPosition.x;
        newArrowPosition.y = -(selectedOption+startIndex)*35;
        newArrowPosition.z = arrow.transform.localPosition.z;

        arrow.transform.localPosition = newArrowPosition;


        if(mainPlayer.GetInput().GetConfirmDown()){
            switch(selectedOption){
                case 0:
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                    break;
            }
        }
    }

    void ChangeOption(){
        if(selectedOption >= maxOptions) selectedOption = 0;
        if(selectedOption < 0) selectedOption = maxOptions-1;
        options[selectedOption].color = selectedColor;
    }

    void StopTime(){
        Time.timeScale = 0;
    }
}