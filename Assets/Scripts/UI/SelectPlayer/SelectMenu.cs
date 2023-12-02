using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private List<SelectOption> characters;
    [SerializeField] private List<PlayerSelectionSettings> players;
    private AudioSource audioS;
    [SerializeField] private AudioClip confirmAudio;
    [SerializeField] private AudioClip errorAudio;
    [SerializeField] private AudioClip changeAudio;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.players.Clear();
        for(int i = 0; i < players.Count; i++)
            if(i < characters.Count){
                players[i].characterIndex = -1;
                players[i].selector.transform.position = characters[i].target.position;
                players[i].inputs = GameManager.Instance.playersInputs[i];
            }
        players[0].isActived = true;
        players[0].characterIndex = 0;
        players[0].selector.SetActive(true);
        characters[0].light.SetActive(true);
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < players.Count; i++){

            if(players[i].isReady){
                continue;
            }

            if(players[i].isActived){
                if(!characters[players[i].characterIndex].isSelected)
                    characters[players[i].characterIndex].light.SetActive(false);

                if(Input.GetAxisRaw(players[i].inputs.horizontalAxis) > 0 && !players[i].pressedAxis){
                    players[i].characterIndex++;
                    players[i].pressedAxis = true;
                    PlaySound(changeAudio);
                }else if(Input.GetAxisRaw(players[i].inputs.horizontalAxis) < 0 && !players[i].pressedAxis){
                    players[i].characterIndex--;
                    players[i].pressedAxis = true;
                    PlaySound(changeAudio);
                }else if(Input.GetAxisRaw(players[i].inputs.horizontalAxis) == 0) players[i].pressedAxis = false;

                if(players[i].characterIndex >= characters.Count) players[i].characterIndex = characters.Count-1;
                if(players[i].characterIndex < 0) players[i].characterIndex = 0;
                players[i].selector.transform.position = characters[players[i].characterIndex].target.position;
                characters[players[i].characterIndex].light.SetActive(true);
                
                if(Input.GetButtonDown(players[i].inputs.confirm)){
                    if(characters[players[i].characterIndex].isSelected){
                        PlaySound(errorAudio, 0.8f);
                        continue;
                    }
                    PlaySound(confirmAudio);
                    characters[players[i].characterIndex].isSelected = true;
                    characters[players[i].characterIndex].light.SetActive(true);
                    characters[players[i].characterIndex].light.GetComponent<Light2D>().intensity = 0.3f;
                    players[i].selector.SetActive(false);
                    players[i].isReady = true;
                    players[i].playerPrefab = characters[players[i].characterIndex].player;
                    VerifyAllPlayers();
                }
            }
            if(Input.GetButtonDown(players[i].inputs.confirm) && !players[i].isActived){
                players[i].selector.SetActive(true);
                players[i].isActived = true;
                players[i].characterIndex = characters.Count-1;
            }
        }

    }

    void PlaySound(AudioClip audio, float volume = 0.4f){
        if(!audioS.isPlaying){
            audioS.PlayOneShot(audio, volume);
        }
    }

    void VerifyAllPlayers(){
        bool allReady = true;
        for(int i = 0; i < players.Count; i++){
            if(!players[i].isReady && players[i].isActived){
                allReady = false;
                break;
            }
            if(players[i].isReady) GameManager.Instance.players.Add(players[i].playerPrefab);
        }
        if(!allReady) return;

        loading.SetActive(true);
        StartCoroutine(LoadAsyncScene(2));
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
}
