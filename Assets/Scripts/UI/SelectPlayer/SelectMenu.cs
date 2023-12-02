using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PlayerSelectionSettings{
    public InputMap inputs;
    public int characterIndex;
    public GameObject selector;
    public bool isReady = false;
    public bool pressedAxis = false;
}

[System.Serializable]
public class SelectOption{
    public bool isSelected = false;
    public Transform target;
    public GameObject light;
    public GameObject player;
}
public class SelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private List<SelectOption> characters;
    [SerializeField] private List<PlayerSelectionSettings> players;
    [SerializeField] private PlayerSelectionSettings player2;
    private AudioSource audioS;
    [SerializeField] private AudioClip confirmAudio;
    [SerializeField] private AudioClip errorAudio;
    [SerializeField] private AudioClip changeAudio;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < players.Count; i++)
            if(i < characters.Count){
                players[i].characterIndex = i;
                players[i].selector.SetActive(true);
                players[i].selector.transform.position = characters[i].target.position;
                characters[i].light.SetActive(true);
            }
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < players.Count; i++){
            if(players[i].isReady){
                continue;
            }

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
                VerifyAllPlayers();
            }
        }

        if(Input.GetButtonDown(player2.inputs.confirm) && !players.Contains(player2)){
            players.Add(player2);
            player2.selector.SetActive(true);
            player2.characterIndex = characters.Count-1;
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
            if(!players[i].isReady){
                allReady = false;
                break;
            }
        }
        if(!allReady) return;

        GameManager.Instance.players = players;
        loading.SetActive(true);
    }
    
}
