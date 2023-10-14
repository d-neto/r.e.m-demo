using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputHandler : MonoBehaviour
{
    public delegate void InputEvent(string button);
    public static event InputEvent OnInput;
    public static event InputEvent OnHold;

    void Update(){

        if(OnInput != null){
            if(Input.GetButtonDown("Fire1")) OnInput("Fire1");
            if(Input.GetButtonDown("Fire2")) OnInput("Fire2");
            if(Input.GetButtonDown("Fire3")) OnInput("Fire3");
        }
    }
}