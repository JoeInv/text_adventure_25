using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> inventory = new List<string>();

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
        InputManager.instance.onRestart += ResetGame; // ResetGame() will be code to respond to event
        Load();
    }

    void Load() 
    {
        if(File.Exists(Application.persistentDataPath + "/player.save")) //Existing player
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveState playerData = (SaveState) bf.Deserialize(afile);
            afile.Close();

            Room room = NavigationManager.instance.GetRoomFromName(playerData.currentRoom);
            if (room != null)
            {
                NavigationManager.instance.SwitchRooms(room);
            }
            inventory.Clear();
            if (playerData.inventory != null)
                inventory.AddRange(playerData.inventory);
            
        }
        else //New player!!!
        {
            NavigationManager.instance.ResetGame();
        }
    }

    void ResetGame()
    {
        inventory.Clear();
    }

    public void Save()
    {
        //set up data to save
        SaveState playerState = new SaveState();
        playerState.currentRoom = NavigationManager.instance.currentRoom.name;
        playerState.inventory = new List<string>(inventory);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream afile = File.Create(Application.persistentDataPath + "/player.save");
        Debug.Log(Application.persistentDataPath);
        bf.Serialize(afile, playerState);
        afile.Close();
    }

}
