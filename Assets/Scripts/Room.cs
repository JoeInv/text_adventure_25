using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text/Room")]
public class Room : ScriptableObject
    // Start is called before the first frame update
{
    public string roomName;
    [TextArea]
    public string description;
    public Exit[] exits;
    
    public bool hasKey;
    public bool hasOrb;
    
}

