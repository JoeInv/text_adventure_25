using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text/Exit")]
public class Exit : ScriptableObject
{
    public enum Direction {north,east,south,west};
    public Direction direction;
    [TextArea]
    public string description;
    public Room room;

    public bool isLocked;
    public bool isHidden;
}
