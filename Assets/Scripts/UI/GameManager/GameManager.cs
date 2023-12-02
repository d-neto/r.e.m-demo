using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public List<InputMap> playersInputs;
    public List<GameObject> players = new List<GameObject>(2);
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

}
