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
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>(); // holds the commands entered by the user

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
        userInput.onEndEdit.AddListener(GetInput);
        story = storyText.text;
    }

    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;
    }

    void GetInput(string msg)
    {
        if (msg != "")
        {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo);

            if (commands.Contains(parts[0]))
            {
                if (parts[0] == "go")
                {
                    if(NavigationManager.instance.SwitchRooms(parts[1]))
                    {
                        
                    }
                    else
                    {
                        UpdateStory("There is no exit in that direction.");
                    }
                }
                else if(parts[0] == "get")
                {
                    if(NavigationManager.instance.TakeItem(parts[1]))
                    {
                        GameManager.instance.inventory.Add(parts[1]);
                        UpdateStory("You picked up the " + parts[1]);
                    }
                    else
                    {
                        UpdateStory("Sorry, " + parts[1] + " is not here.");
                    }  
                }
            }
        }
        userInput.text = "";
        userInput.ActivateInputField();
    }
}
