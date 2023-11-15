// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;
    public void infiniteMode(){
        SceneManager.LoadScene(nomeDoLevelDeJogo);
    }

    public void storyMode(){
        Debug.Log("Botão desabilitado");
    }

    public void credits(){
        Debug.Log("Créditos");
    }

    public void quit(){
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    void Update(){
        if(Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){
            infiniteMode();
        }
    }
}
