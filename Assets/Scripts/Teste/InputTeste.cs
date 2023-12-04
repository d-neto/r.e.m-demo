using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTeste : MonoBehaviour
{

    void Update()
    {
        if(Input.GetButtonDown("Joystick1:Confirm")) Debug.Log("APERTADO 1");
        if(Input.GetButtonDown("Joystick2:Confirm")) Debug.Log("APERTADO 2 TBM!");
    }
}
