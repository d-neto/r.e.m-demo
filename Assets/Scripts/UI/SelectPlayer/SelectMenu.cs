using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMenu : MonoBehaviour
{
    public Image herculesImage, luciusImage;
    public Animator herculesAnim, luciusAnim;
    
    public Animator playselect1, playselect2;
    private Color defaultColor;
    private int characterIndex;
    // Start is called before the first frame update
    void Start()
    {
        characterIndex = 1;
        defaultColor = luciusImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            characterIndex = 1;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)){
            characterIndex = 2;
        }

        if(characterIndex == 1){
            herculesImage.color = Color.yellow;
            herculesAnim.SetBool("idle",true);
            luciusImage.color = defaultColor;
            luciusAnim.SetBool("idle",false);
        }
        else if(characterIndex == 2){
            luciusImage.color = Color.yellow;
            luciusAnim.SetBool("idle",true);
            herculesImage.color = defaultColor;
            herculesAnim.SetBool("idle",false);
        }

    }
}
