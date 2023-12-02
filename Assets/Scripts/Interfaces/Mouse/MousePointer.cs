using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public Transform pointer;
    private void Start(){
        Cursor.visible = false;
    }
    private void Update(){
        Vector3 mouse = Input.mousePosition;
        mouse.z = 0.5f;
        pointer.position = mouse;
    }

    public void DisableMouse(){
        Cursor.visible = false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus){
            Cursor.visible = false;
        }
    }
}
