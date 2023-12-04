using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject startText;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject loading;
    [SerializeField] GameObject options;
    [SerializeField] AudioClip startAudio;
    [SerializeField] AudioClip optionAudio;
    [SerializeField] List<InputMap> allInputs;

    bool isStarted = false;

    void Start(){
        Cursor.visible = true;
    }
    void Update(){
        if(!isStarted && Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){
            audioSource.PlayOneShot(startAudio, 0.8f);
            startText.SetActive(false);
            menu.SetActive(true);
            isStarted = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            options.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void InifiniteMode(){
        Cursor.visible = false;
        loading.SetActive(true);
        StartCoroutine(LoadAsyncScene(1));
    }

    IEnumerator LoadAsyncScene(int scene)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void PlayOptionAudio(){
        this.audioSource.PlayOneShot(optionAudio, 0.1f);
    }
    public void PlayClickAudio(){
        this.audioSource.PlayOneShot(optionAudio, 0.1f);
    }

    public void ChangePlayer1Inputs(int index){
        GameManager.Instance.playersInputs[0] = allInputs[index];
    }
    public void ChangePlayer2Inputs(int index){
        GameManager.Instance.playersInputs[1] = allInputs[index];
    }
    public void ShowOptions(){
        menu.SetActive(false);
        options.SetActive(true);
    }
    public void QuitGame(){
        Application.Quit();
    }
}
