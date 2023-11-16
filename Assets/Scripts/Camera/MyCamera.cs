using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MyCamera : MonoBehaviour {
    
    public static MyCamera Instance {get; private set;}

    [SerializeField] private Camera cam;
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    CinemachineBasicMultiChannelPerlin noise;
    private void Awake() {
        if(!Instance) Instance = this;
        else{
            Destroy(this);
            return;
        }

        noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void SmoothShake(){
        if (noise != null) {
            noise.m_AmplitudeGain = 0.5f;
            noise.m_FrequencyGain = 0.5f;
            StartCoroutine(StopShake(0.2f));
        }
    }

    public void StrongShake(){
        if (noise != null) {
            noise.m_AmplitudeGain = 1.4f;
            noise.m_FrequencyGain = 1f;
            StartCoroutine(StopShake(0.3f));
        }
    }

    public void ShakeCam(float duration, float amplitude, float frequency) {
        if (noise != null) {
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
            StartCoroutine(StopShake(duration));
        }
    }
    IEnumerator StopShake(float duration) {
        yield return new WaitForSeconds(duration);
        if (noise != null) {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}