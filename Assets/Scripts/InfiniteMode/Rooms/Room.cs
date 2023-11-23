using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int orderGenerated {get; private set;}
    [SerializeField] private GameObject actualGrid;
    [SerializeField] List<RoomSpawnPoint> enterPoints;
    public List<Room> connections = new List<Room>();
    public bool isAnyPlayerHere = false;
    bool generated = false;
    char connectedWith = '0';
    public List<Room> Generate(){
        if(generated) return new List<Room>();
        List<Room> rooms = new List<Room>();
        for(int i = 0; i < enterPoints.Count; i++){
            Room newRoom = Spawn(enterPoints[i]);
            if(newRoom != null){
                newRoom.connections.Add(this);
                rooms.Add(newRoom);
                connections.Add(newRoom);
            }
        }
        generated = true;
        return rooms;
    }

    public void SetupRandom(RoomGroup rooms){
        for(int i = 0; i < enterPoints.Count; i++){
            if(enterPoints[i].isConnected) continue;
            List<GameObject> canConnect = rooms.GetAll();
            if(canConnect.Count == 0) continue;
            int index = Random.Range(0, canConnect.Count);
            enterPoints[i].spawnPrefab = canConnect[index];
        }
    }

    Room Spawn(RoomSpawnPoint point){
        if(point.isConnected){
            return null;
        }

        Vector2 spawn = point.transform.position;
        GameObject clone = null;
        Room cloneRoom = null;
        RoomSpawnPoint connection = null;

        if(point.spawnPrefab != null){
            clone = Instantiate(point.spawnPrefab, spawn, point.transform.rotation);
            cloneRoom = clone.GetComponent<Room>();
            switch(point.direction){
                case 'R': connection = cloneRoom.Get('L');
                    break;
                case 'L': connection = cloneRoom.Get('R');
                    break;
                case 'T': connection = cloneRoom.Get('B');
                    break;
                case 'B': connection = cloneRoom.Get('T');
                    break;
            }
        }
        cloneRoom.SetConnection(point.direction);

        float distY = point.transform.position.y - connection.transform.position.y;
        float distX = point.transform.position.x - connection.transform.position.x;
        clone.transform.Translate(distX, distY, 0);
        clone.transform.SetParent(actualGrid.transform);
        cloneRoom.SetGrid(this.actualGrid);
        point.isConnected = true;
        connection.isConnected = true;
        return cloneRoom;
    }

    public RoomSpawnPoint Get(char dir){
        foreach(RoomSpawnPoint point in enterPoints)
            if(point.direction == dir)
                return point;
        return null;
    }

    public void SetGrid(GameObject grid) => this.actualGrid = grid;
    public void SetConnection(char dir) => this.connectedWith = dir;
    public void SetOrder(int value) => this.orderGenerated = value;

    int playersInRoom = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            RoomGenerator.Instance.VerifyGeneration(this);
            playersInRoom++;
            isAnyPlayerHere = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            playersInRoom--;
            if(playersInRoom <= 0){
                playersInRoom = 0;
                isAnyPlayerHere = false;
            }
        }
    }

    public bool WasGenerated() => this.generated;

}
