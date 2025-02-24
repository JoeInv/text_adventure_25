using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;
    public Room startingRoom;
    public Room currentRoom;

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
        Debug.Log(startingRoom.roomName);
        Debug.Log(startingRoom.description);
        
    }
}
