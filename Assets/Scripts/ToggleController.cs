using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Image background;
    public Text displayText;
    public Text themeText;
    public Text fontText;
    public Text placeholderText;
    public Text playerText; //what user types

    public Toggle themeToggle;
    public Toggle fontToggle;
    private bool darkMode;
    private bool largeFont;


    // Start is called before the first frame update
    void Start()
    {
        int themePref = PlayerPrefs.GetInt("theme", 1);
        int fontPref = PlayerPrefs.GetInt("fontSize", 0);
        darkMode = themePref == 1;
        themeToggle.isOn = darkMode;
        largeFont = fontPref == 1;
        fontToggle.isOn = largeFont;

        SetTheme();
        SetFontSize();

        themeToggle.onValueChanged.AddListener(DarkMode);
        fontToggle.onValueChanged.AddListener(FontSize);
    }

    void DarkMode(bool value)
    {
        darkMode = value;
        PlayerPrefs.SetInt("theme", darkMode ? 1 : 0);
        SetTheme();
    }

    void FontSize(bool value)
    { 
        largeFont = value;
        PlayerPrefs.SetInt("fontSize", largeFont ? 1 : 0);
        SetFontSize(); 
    }
    void SetTheme()
    {
        if (darkMode)
        {
            background.color = Color.black;
            displayText.color = Color.white;
            themeText.color = Color.white;
            fontText.color = Color.white;
            placeholderText.color = Color.white;
            playerText.color = Color.white;
        }
        else
        {
            background.color = Color.white;
            displayText.color = Color.black;
            themeText.color = Color.black;
            fontText.color= Color.black;
            placeholderText.color = Color.black;
            playerText.color = Color.black;
        }
    }

    void SetFontSize()
    { 
        int fontSize = largeFont ? 26 : 18;

        displayText.fontSize = fontSize;
        themeText.fontSize = fontSize;
        fontText.fontSize = fontSize;
        placeholderText.fontSize = fontSize;
        playerText.fontSize = fontSize;

    }
  
}
