using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontController : MonoBehaviour
{
    public Image background;
    public Text displayText;
    public Text toggleText;
    public Text placeholderText;
    public Text playerText; //what user types

    private bool largeFont;

    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        //darkMode = toggle.isOn;

        int pref = PlayerPrefs.GetInt("theme", 1); //uses 1 as default if not already set
        if (pref == 1) //darkmode is the preference
        {
            toggle.isOn = true;
            largeFont = true;
        }
        else //otherwise, go with lightmode
        {
            toggle.isOn = false;
            largeFont = false;
        }

        SetFontSize();
        toggle.onValueChanged.AddListener(FontSize);
    }

    void FontSize(bool value)
    {
        largeFont = value;
        PlayerPrefs.SetInt("FontSize", largeFont ? 1 : 0);
        SetFontSize();
    }

    void SetFontSize()
    {
        int fontSize = largeFont ? 24 : 16;
        displayText.fontSize = fontSize;
        toggleText.fontSize = fontSize;
        placeholderText.fontSize = fontSize;
    }


}
