using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int characterIndex;
    public static GameManager Instance {get; private set;}
    public List<PlayerSelectionSettings> players;
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

}
