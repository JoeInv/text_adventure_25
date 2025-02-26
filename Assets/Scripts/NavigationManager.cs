using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    public Exit toKeyNorth;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Start()
    {
        currentRoom = startingRoom;
        toKeyNorth.isHidden = true;
        Unpack();
        
    }

    void Unpack()
    {
        string description = currentRoom.description;
        exitRooms.Clear();
        foreach(Exit e in currentRoom.exits)
        {
            if(!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);
    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction))
        {
            currentRoom = exitRooms[direction];
            InputManager.instance.UpdateStory("You move to the " + direction);
            Unpack();
            return true;
        }
        return false;
    }

    public bool TakeItem(string item)
    {
        if(item == "key" && currentRoom.hasKey)
        {
            return true;
        }
        else if (item == "orb" && currentRoom.hasOrb)
        {
            toKeyNorth.isHidden = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
