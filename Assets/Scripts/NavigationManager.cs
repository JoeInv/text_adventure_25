using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;

    public Exit toKeyNorth; //needed to turn exit to visible from hidden

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void RestartGame()
    {
        toKeyNorth.isHidden = true;
        currentRoom = startingRoom;
        Unpack();
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.OnRestart += RestartGame;
        //Debug.Log(startingRoom.description);
    }

    void Unpack()
    {
        string description = currentRoom.description;
        exitRooms.Clear(); //reset for next room
        foreach(Exit e in currentRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);

    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction)) //if that exit exists
        {
            if(!getExit(direction).isLocked || GameManager.instance.inventory.Contains("key"))
            {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                Unpack();
                return true;
            }
        }

        return false;
    }

    Exit getExit(string direction)
    {
        foreach (Exit e in currentRoom.exits)
        {
            if (e.direction.ToString() == direction)
                return e;
        }
        return null;
    }

    public bool TakeItem(string item)
    {
        if (item == "key" && currentRoom.hasKey)
            return true;
        else if (item == "orb" && currentRoom.hasOrb)
        {
            toKeyNorth.isHidden = false;
            return true;
        }
        else
            return false;
    }
}
