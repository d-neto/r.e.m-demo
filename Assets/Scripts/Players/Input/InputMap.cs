using UnityEngine;

[CreateAssetMenu(fileName = "newInputMap", menuName = "Input Map/ New Input")]
public class InputMap : ScriptableObject {
    public string horizontalAxis;
    public string verticalAxis;
    public string dash;
    public string dropObject;
    public string pickObject;
    public string shoot;
    public string reload;
    public string switchTarget;

}