using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    public Text[] textUI;
    public string[] text_eng, text_rus;
    public string lang;
    public Sprite eng, rus;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            lang = PlayerPrefs.GetString("Language");
        }
        else
        {
            lang = "ENG";
        }

        ChangeText();
    }

    
    public void ChangeLang()
    {
        if(lang=="ENG")
        {
            lang = "RUS";
            PlayerPrefs.SetString("Language", lang);
            ChangeText();
        }
        else
        {
            lang = "ENG";
            PlayerPrefs.SetString("Language", lang);
            ChangeText();
        }
    }

    public void ChangeText()
    {
        if (lang == "ENG")
        {
            gameObject.GetComponent<Image>().sprite = eng;
            for (int i = 0; i < textUI.Length; i++)
            {

                textUI[i].GetComponent<Text>().text = text_eng[i];
            }
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = rus;
            for (int i = 0; i < textUI.Length; i++)
            {

                textUI[i].GetComponent<Text>().text = text_rus[i];
            }
        }
    }
}
