using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    public List<Room> rooms; //needed to restore room upon load

    // new delegate
    public delegate void GameOver();
    public event GameOver onGameOver;

    public Exit toKeyNorth; //needed to turn exit to visible from hidden
    public Exit toSmokeyWest;
    public Exit toDecisionEast;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.onRestart += ResetGame;
        //Debug.Log(startingRoom.description);
        //toKeyNorth.isHidden = true;
        //currentRoom = startingRoom;
        //Unpack();
        //ResetGame();
    }

    public void ResetGame()
    {
        toKeyNorth.isHidden = true;
        toSmokeyWest.isHidden = true;
        toDecisionEast.isHidden = true;
        currentRoom = startingRoom;
        Unpack();
    }

    void Unpack()
    {
        string description = currentRoom.description;
        exitRooms.Clear(); //reset for next room
        foreach (Exit e in currentRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);
        if (exitRooms.Count == 0)
            if (onGameOver != null) //is anyone listening
                if (GameManager.instance.inventory.Contains("amulet"))
                {
                    if (currentRoom.roomName != "win")
                    {
                        GameManager.instance.inventory.Remove("amulet");
                        GameManager.instance.inventory.Add("broken amulet");
                        InputManager.instance.UpdateStory("The amulet seems to have saved your life, but has broken in the process.");
                        if (currentRoom.roomName == "dragon")
                        {
                            //Debug.Log("unlocked");
                            toSmokeyWest.isHidden = false;
                            foreach (Exit e in currentRoom.exits)
                            {
                                if (!e.isHidden)
                                {
                                    description += " " + e.description;
                                    exitRooms.Add(e.direction.ToString(), e.room);
                                }
                            }
                            InputManager.instance.UpdateStory("A door has opened to the west. Escape while you can!");
                        }
                        else if (currentRoom.roomName == "trap")
                        {
                            //Debug.Log("unlocked");
                            toDecisionEast.isHidden = false; exitRooms.Clear(); //reset for next room
                            foreach (Exit e in currentRoom.exits)
                            {
                                if (!e.isHidden)
                                {
                                    description += " " + e.description;
                                    exitRooms.Add(e.direction.ToString(), e.room);
                                }
                            }
                            InputManager.instance.UpdateStory("A door has opened to the east. Escape while you can!");
                            
                        }
                    }
                    else
                        onGameOver(); //invoking the event
                }
    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction)) //if that exit exists
        {
            if (!getExit(direction).isLocked  || GameManager.instance.inventory.Contains("key")) //check to see if exit is locked - only proceed if not locked
            {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                Unpack();
                return true;
            }
        }

        return false;
    }

    public void SwitchRooms(Room room)
    {
        currentRoom = room;
        Unpack();
    }

    Exit getExit(string direction)
    {
        foreach (Exit e in currentRoom.exits)
        {
            if (e.direction.ToString() == direction)
                return e; //returns exit
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
        else if (item == "amulet" && currentRoom.hasAmulet)
            return true;
        else
            return false;
    }

    public Room GetRoomFromName(string name)
    {
        foreach(Room aRoom in rooms)
        {
            if(aRoom.name == name)
                return aRoom;
        }

        return null;
    }

}
