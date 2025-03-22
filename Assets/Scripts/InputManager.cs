using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Text storyText; // the story 
    public InputField userInput; // the input field object
    public Text inputText; // part of the input field where user enters response
    public Text placeHolderText; // part of the input field for initial placeholder text
    //public Button abutton;

    //1st step to creating and using delegate
    public delegate void Restart(); //create delegate
    public event Restart onRestart; //create event
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>(); //valid user commands

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");
        commands.Add("commands");
        commands.Add("inventory");
        commands.Add("restart");  // added to work with delegate example
        commands.Add("save");

        userInput.onEndEdit.AddListener(GetInput); //now calls GetInput
        //abutton.onClick.AddListener(DoSomething);
        story = storyText.text;
        NavigationManager.instance.onGameOver += EndGame; //function to call when event occurs
    }

    void EndGame() //function to respond to the event
    {
        UpdateStory("\nPlease enter 'restart' to play again. ");
    }

    //void DoSomething() //event handler
    //{
    //    Debug.Log("Button clicked!");
    //}

    public void UpdateStory(string msg) //update display
    {
        story += "\n" + msg;
        storyText.text = story;
    }

    void GetInput(string msg) //process input
    {
        if (msg != "")
        {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo); //['go', 'north']

            if (commands.Contains(parts[0])) //if valid command
            {
                if (parts[0] == "go") //wants to switch rooms
                {
                    if (NavigationManager.instance.SwitchRooms(parts[1])) //returns true if direction exits
                    {
                        //fill in later
                    }
                    else
                    {
                        // added the "is locked" response
                        UpdateStory("Exit does not exist or is locked. Try again.");
                    }
                }
                else if(parts[0] == "get") //wants to add item to inventory
                {
                    if (NavigationManager.instance.TakeItem(parts[1])) //returns true if direction exits
                    {
                        GameManager.instance.inventory.Add(parts[1]);
                        UpdateStory("You added a(n) " + parts[1] + " to your inventory.");
                    }
                    else
                    {
                        UpdateStory("Sorry, " + parts[1] + " does not exist in this room.");
                    }
                }
                else if(parts[0] == "restart")
                {
                    if (onRestart != null) // if anyone is listening
                        onRestart(); //invoke the event
                }
                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                }
                else if (parts[0] == "inventory")
                {
                    string currentInv = string.Join(", ", GameManager.instance.inventory);
                    UpdateStory("Inventory: " + currentInv);
                }
                else if (parts[0] == "commands")
                {
                    string listoCommands = string.Join (", ", commands);
                    UpdateStory("Commands: " + listoCommands);
                }
            }
        }

        // reset for next input
        userInput.text = "";
        userInput.ActivateInputField();
    }

}
