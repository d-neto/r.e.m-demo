using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class JoystickVibration : MonoBehaviour
{
    public static JoystickVibration Instance;

    void Awake(){
        if(Instance == null)
            Instance = this;
        else Destroy(this);
    }

    public void Rumble(int index, float lowFrequency, float highFrequency, float timing){
        if(Gamepad.all.Count <= index || index < 0) return;
        Gamepad.all[index]?.SetMotorSpeeds(lowFrequency, highFrequency);
        StartCoroutine(StopRumble(index, timing));
    }

    IEnumerator StopRumble(int index, float timing){
        yield return new WaitForSeconds(timing);
        Stop(index);
    }

    public void Stop(int index){
        if(Gamepad.all.Count <= index || index < 0) return;
        Gamepad.all[index]?.SetMotorSpeeds(0f, 0f);
    }

    public void DisableAll(){
        for(int i = 0; i < Gamepad.all.Count; i++){
            Gamepad.all[i]?.SetMotorSpeeds(0f, 0f);
        }
    }

    private void OnApplicationQuit() {
        for(int i = 0; i < Gamepad.all.Count; i++){
            Gamepad.all[i]?.SetMotorSpeeds(0f, 0f);
        }
    }
}