using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    

    void Update(){
        if(Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
