using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomGenerator : MonoBehaviour
{
    
    public static RoomGenerator Instance;
    public List<Room> rooms = new List<Room>();

    public RoomGroup roomsPrefabs;

    [SerializeField] private int initialIterations;
    [SerializeField] private int actualSizeOfGenerateds = 0;
    [SerializeField] private int chunkSize = 16;

    bool initStarted = false;
    float valuePerRoom;
    public Animator loadingAnimator;
    public Slider loadingSlider;
    public TMP_Text loadingText;

    void Awake(){
        if(!Instance) Instance = this;
        else Destroy(this);

        roomsPrefabs = Instantiate(roomsPrefabs);
    }

    void Start(){
        if(!initStarted)
            StartCoroutine(StartGenerate());
    }

    IEnumerator StartGenerate(){
        initStarted = true;
        valuePerRoom = 100f/initialIterations;
        float loaded = 10;
        SetSlider(loaded);
        for(int i = 0; i < initialIterations && i < rooms.Count; i++){
            rooms[i].SetupRandom(roomsPrefabs);
            yield return StartCoroutine(GenerateRoomAsync(rooms[i]));
            loaded += valuePerRoom;
            if(loaded > 100) loaded = 100;
            loadingText.text = (int) loaded + "%";
            SetSlider(loaded);
        }
        loadingAnimator.SetBool("disapear", true);
        AstarPath.active.FlushGraphUpdates();
    }

    IEnumerator Generate(int size){
        int maxSize = actualSizeOfGenerateds+size;
        for(int i = actualSizeOfGenerateds-1; i < maxSize && i < rooms.Count; i++){
            rooms[i].SetupRandom(roomsPrefabs);
            yield return StartCoroutine(GenerateRoomAsync(rooms[i]));
        }
    }
    
    IEnumerator GenerateByChunk(Room room, int size){
        List<Room> generateds = new List<Room>();
        List<Room> generatedByCurrent = null;
        
        generateds.Add(room);

        for(int i = 0; i < size && i < generateds.Count; i++){
            if(generateds[i].WasGenerated()){
                for(int j = 0; j < generateds[i].connections.Count; j++) 
                    generateds.Add(generateds[i].connections[j]);
                continue;
            }
            generateds[i].SetupRandom(roomsPrefabs);

            generatedByCurrent = generateds[i].Generate();
            if(generatedByCurrent.Count == 0) generatedByCurrent = generateds[i].connections;
            for(int j = 0; j < generatedByCurrent.Count; j++){
                rooms.Add(generatedByCurrent[j]);
                generateds.Add(generatedByCurrent[j]);
                generatedByCurrent[j].SetOrder(actualSizeOfGenerateds);
                actualSizeOfGenerateds++;
                yield return null;
            }
        }
        AstarPath.active.FlushGraphUpdates();
        yield return null;
    }

    void GenerateChunks(){
        StartCoroutine(Generate(chunkSize));
    }
    void GenerateChunksByIndex(Room room){
        StartCoroutine(GenerateByChunk(room, chunkSize));
    }

    IEnumerator GenerateRoomAsync(Room room)
    {
        foreach (Room generatedRoom in room.Generate())
        {
            rooms.Add(generatedRoom);
            generatedRoom.SetOrder(actualSizeOfGenerateds);
            actualSizeOfGenerateds++;
            yield return null;
        }
    }

    void SetSlider(float value){
        if(loadingSlider) loadingSlider.value = value;
    }
    void UpdateSlider(float value){
        if(loadingSlider) loadingSlider.value += value;
    }

    public void VerifyGeneration(Room room){
        room.isAnyPlayerHere = true;
        if(!room.WasGenerated()) GenerateChunksByIndex(room);
        int generatedsAround = 0;
        List<Room> generateds = new List<Room>();
        generateds.Add(room);
        for(int i = 0; i < 12 && i < generateds.Count; i++){
            if(generateds[i].connections == null) continue;
            for(int j = 0; j < generateds[i].connections.Count; j++){
                if(generateds[i].connections[j].WasGenerated()) generatedsAround++;
                generateds.Add(generateds[i].connections[j]);
            }
        }
        if(generatedsAround <= 64){
            GenerateChunksByIndex(room);
        }
        SetVisiblesByPlayersPosition();
    }

    void SetVisiblesByPlayersPosition(){
        List<Room> render = new List<Room>();
        for(int k = 0; k < rooms.Count; k++){
            if(IsRoomNearPlayer(rooms[k])){
                render.Add(rooms[k]);
                rooms[k].gameObject.SetActive(true);
            }
        }
        for(int i = 0; i < rooms.Count; i++){
            if(!render.Contains(rooms[i])) rooms[i].gameObject.SetActive(false);
        }
    }

    bool IsRoomNearPlayer(Room room)
    {
        foreach (Player player in PlayerManager.Instance.players)
        {
            float distance = Vector2.Distance(room.transform.position, player.transform.position);
            if (distance < 32*4){
                return true;
            }
        }
        return false;
    }
}
