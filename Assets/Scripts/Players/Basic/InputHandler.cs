using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputHandler : MonoBehaviour
{
    public delegate void InputEvent();
    public delegate void GunEvent(Gun gun);
    public static event InputEvent OnInputFire;
    public static event GunEvent OnReloadStart;
    public static event GunEvent OnReloadEnd;

    void Update(){
        if(OnInputFire != null){
            if(Input.GetButton("Fire1")) OnInputFire();
        }
    }

    public static void Reloading(Gun gun, bool start){
        if(start) OnReloadStart?.Invoke(gun);
        else OnReloadEnd?.Invoke(gun);
    }
}