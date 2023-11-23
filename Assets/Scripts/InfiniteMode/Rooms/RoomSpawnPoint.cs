using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour{

    public Room room;
    public char direction;
    public GameObject spawnPrefab;
    public bool isConnected = false;

    void Start(){
        if(!room) room = transform.parent.parent.GetComponent<Room>();
    }

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject.layer == 29 && !isConnected){

            isConnected = true;
            c.gameObject.GetComponent<RoomSpawnPoint>().isConnected = true;

            c.gameObject.GetComponent<RoomSpawnPoint>().room.connections.Add(room);
            room.connections.Add(c.gameObject.GetComponent<RoomSpawnPoint>().room);

            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}