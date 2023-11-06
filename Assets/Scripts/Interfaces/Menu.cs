using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame(){
        //Editor Unity
        UnityEditor.EditorApplication.isPlaying = false;
        //Jogo Compilado
        //Application.Quit();
    }
}
