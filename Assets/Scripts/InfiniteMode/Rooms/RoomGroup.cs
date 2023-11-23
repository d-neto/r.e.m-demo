using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newRoomListData", menuName = "Data/Maps Data/Room 4DirRooms")]
public class RoomGroup : ScriptableObject{
    public List<GameObject> topRooms;
    public List<GameObject> rightRooms;
    public List<GameObject> bottomRooms;
    public List<GameObject> leftRooms;

    public List<GameObject> Get(char dir){
        return dir switch
        {
            'T' => bottomRooms,
            'R' => leftRooms,
            'B' => topRooms,
            'L' => rightRooms,
            _ => null,
        };
    }

    public List<GameObject> GetAll(){
        List<GameObject> objects = new List<GameObject>(topRooms);
        objects.Concat(bottomRooms);
        objects.Concat(leftRooms);
        objects.Concat(rightRooms);
        return objects;
    }
}