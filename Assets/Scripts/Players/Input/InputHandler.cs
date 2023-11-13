using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputHandler : MonoBehaviour
{
    public delegate void InputEvent();
    public delegate void GunEvent(Gun gun);
    public event InputEvent OnInputFire;
    public event GunEvent OnReloadStart;
    public event GunEvent OnReloadEnd;
    [SerializeField] private InputMap InputButtons;

    void Awake(){
        InputButtons = Instantiate(InputButtons);
    }

    void Update(){
        if(OnInputFire != null){
            if(GetFire()) OnInputFire();
        }
    }

    public void Reloading(Gun gun, bool start){
        if(start) OnReloadStart?.Invoke(gun);
        else OnReloadEnd?.Invoke(gun);
    }

    public InputMap Get() => this.InputButtons;
    public float GetHorizontal() => Input.GetAxis(InputButtons.horizontalAxis);
    public float GetHorizontalRaw() => Input.GetAxisRaw(InputButtons.horizontalAxis);
    public float GetVertical() => Input.GetAxis(InputButtons.verticalAxis);
    public float GetVerticalRaw() => Input.GetAxisRaw(InputButtons.verticalAxis);
    public bool GetFire() => Input.GetButton(InputButtons.shoot);
    public bool GetFireDown() => Input.GetButtonDown(InputButtons.shoot);
    public bool GetReload() => Input.GetButton(InputButtons.reload);
    public bool GetDash() => Input.GetButton(InputButtons.dash);
    public bool GetSwitchTarget() => Input.GetButton(InputButtons.switchTarget);
    public bool GetSwitchTargetDown() => Input.GetButtonDown(InputButtons.switchTarget);
    public bool GetDropObject() => Input.GetButton(InputButtons.dropObject);
    public bool GetPickObject() => Input.GetButton(InputButtons.pickObject);
    public bool GetConfirmDown() => Input.GetButtonDown(InputButtons.confirm);
    public bool GetConfirm() => Input.GetButton(InputButtons.confirm);
    public bool GetPauseDown() => Input.GetButtonDown(InputButtons.pause);
}

