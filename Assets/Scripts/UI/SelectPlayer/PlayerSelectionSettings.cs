using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSelectionSettings{
    public InputMap inputs;
    public int characterIndex;
    public GameObject selector;
    public GameObject playerPrefab;
    public bool isReady = false;
    public bool isActived = false;
    public bool pressedAxis = false;
}