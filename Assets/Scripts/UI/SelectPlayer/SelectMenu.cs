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
    private AudioSource audioS;
    // Start is called before the first frame update
    void Start()
    {
        characterIndex = 1;
        audioS = GetComponent<AudioSource>();
        defaultColor = luciusImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            characterIndex = 1;
            PlaySound();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)){
            characterIndex = 2;
            PlaySound();
        }

        if(characterIndex == 1){
            herculesImage.color = Color.yellow;
            herculesAnim.SetBool("idle",true);
            playselect1.SetBool("move",true);
            luciusImage.color = defaultColor;
            luciusAnim.SetBool("idle",false);
            playselect2.SetBool("move",false);
        }
        else if(characterIndex == 2){
            luciusImage.color = Color.yellow;
            luciusAnim.SetBool("idle",true);
            playselect2.SetBool("move",true);
            herculesImage.color = defaultColor;
            herculesAnim.SetBool("idle",false);
            playselect1.SetBool("move",false);
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            FindObjectOfType<GameManager>().characterIndex = characterIndex;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    void PlaySound(){
        if(!audioS.isPlaying){
            audioS.Play();
        }
    }
    
}
