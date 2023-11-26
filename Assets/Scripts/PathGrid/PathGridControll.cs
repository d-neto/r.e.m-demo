using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathGridControll : MonoBehaviour
{
    public static PathGridControll Instance;
    [SerializeField] private AstarPath pathFinder;

    void Awake(){
        if(!Instance)
            Instance = this;
        else Destroy(this);
        AstarPath.active.logPathResults = PathLog.None;
    }

    public void Scan() => pathFinder.ScanAsync();
    public AstarPath Grid() => pathFinder;
    public static PathGridControll Get() => Instance;
}
