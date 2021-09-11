using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [HideInInspector] public Cards[] cards;
    public GameObject Data, ButtonText, shirt, title, background, swnext, swprev;
    public GameObject[] prev_cards;
    public Color swcolor;
    
    private int typecards, issound;
    private string lang;
    private int[] card_num;
    // Start is called before the first frame update

    void Start()
    {
        //FindObjectOfType<AudioManager>().Stop("Theme");
        issound = PlayerPrefs.GetInt("SoundOn");
        cards = Data.GetComponent<Data>().DataCrads;
        typecards = PlayerPrefs.GetInt("SetOfCards");
        card_num = cards[typecards].num_of_choose;
        lang = PlayerPrefs.GetString("Language");
        ChangeText(typecards);
        LoadPreview(false);
    }

    public void ChangeText(int type)
    {
        var text = ButtonText.transform.GetChild(0).GetComponent<Text>();
        var gold = ButtonText.GetComponent<MeinMenuUI>().isgold;

        if (type == PlayerPrefs.GetInt("SetOfCards"))
            gold = true;
        else
            gold = false;

        if (lang == "RUS" && !gold)
        {
            text.text = "Выбрать";
            text.color = new Color(255, 255, 255);
        }
        if (lang == "ENG" && !gold)
        {
            text.text = "Select";
            text.color = new Color(255, 255, 255);
        }
        if (lang == "RUS" && gold)
        {
            text.text = "Выбрано";
            text.color = new Color(52, 52, 100);
        }
        if (lang == "ENG" && gold)
        {
            text.text = "Selected";
            text.color = new Color(52, 52, 100);
        }
    }

    public void ChangeThisDeck()
    {
        if (typecards != PlayerPrefs.GetInt("SetOfCards"))
        {
            PlayerPrefs.SetInt("SetOfCards", typecards);
            FindObjectOfType<AudioManager>().Play("ChangeDeck");
            ButtonText.GetComponent<MeinMenuUI>().isgold = true;
            ButtonText.GetComponent<Image>().sprite = ButtonText.GetComponent<MeinMenuUI>().normal_gold;
            ButtonText.GetComponent<Image>().color = new Color(255, 255, 255);
            ChangeText(typecards);
        }
    }

    public void MusicAgain()
    {
        int index = PlayerPrefs.GetInt("SetOfCards");
        issound = PlayerPrefs.GetInt("SoundOn");
        var data = cards[index];
        if (typecards != index)
        {
            if (issound == 1)
            {
                FindObjectOfType<AudioManager>().Stop("Theme");
                FindObjectOfType<AudioManager>().Stop("Effect");
            }
            FindObjectOfType<AudioManager>().sounds[0].clip = data.background_music;
            FindObjectOfType<AudioManager>().sounds[0].source.clip = data.background_music;
            FindObjectOfType<AudioManager>().sounds[1].clip = data.sound_effect;
            FindObjectOfType<AudioManager>().sounds[1].source.clip = data.sound_effect;

            if (issound == 1)
            {
                FindObjectOfType<AudioManager>().Play("Theme");
                FindObjectOfType<AudioManager>().Play("Effect");
            }
        }
    }

    public void LoadPreview(bool first)
    {
        
        var gold = ButtonText.GetComponent<MeinMenuUI>().isgold;
        var data = cards[typecards];
        card_num = data.num_of_choose;
        if(lang == "RUS")
            title.GetComponent<Text>().text = data.name_ru;
        else
            title.GetComponent<Text>().text = data.name;

        background.GetComponent<SpriteRenderer>().color = data.background_color;

        if (typecards != PlayerPrefs.GetInt("SetOfCards"))
        {
            ButtonText.GetComponent<MeinMenuUI>().isgold = false;
            ButtonText.GetComponent<Image>().color = data.background_color;
            ButtonText.GetComponent<Image>().sprite = ButtonText.GetComponent<MeinMenuUI>().normal;
        }
        else
        {
            ButtonText.GetComponent<MeinMenuUI>().isgold = true;
            ButtonText.GetComponent<Image>().color = new Color(255, 255, 255);
            ButtonText.GetComponent<Image>().sprite = ButtonText.GetComponent<MeinMenuUI>().normal_gold;
        }

        shirt.GetComponent<SpriteRenderer>().sprite = data.shirt;

        for(int i = 0; i < prev_cards.Length; i++)
        {
            prev_cards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.faces[card_num[i]].sprite;
            prev_cards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = data.cards_color[i];
            prev_cards[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = data.cards_color[i];
        }
        if (typecards != PlayerPrefs.GetInt("SetOfCards") || first)
        {
            if (issound == 1)
            {
                FindObjectOfType<AudioManager>().Stop("Theme");
                FindObjectOfType<AudioManager>().Stop("Effect");
            }
           
            FindObjectOfType<AudioManager>().sounds[0].clip = data.background_music;
            FindObjectOfType<AudioManager>().sounds[0].source.clip = data.background_music;
            
            FindObjectOfType<AudioManager>().sounds[1].clip = data.sound_effect;
            FindObjectOfType<AudioManager>().sounds[1].source.clip = data.sound_effect;

            if (issound == 1)
            {
                FindObjectOfType<AudioManager>().Play("Effect");
                FindObjectOfType<AudioManager>().Play("Theme");
            }
        }

        if (typecards == cards.Length - 1)
        {
            swnext.GetComponent<UIButtons>().endlist = true;
            swprev.GetComponent<UIButtons>().endlist = false;
            swprev.GetComponent<Image>().color = new Color(255, 255, 255);
            swnext.GetComponent<Image>().color = swcolor;
        }
        if (typecards != cards.Length - 1 && typecards != 0)
        {
            swnext.GetComponent<UIButtons>().endlist = false;
            swnext.GetComponent<Image>().color = new Color(255, 255, 255);
            swprev.GetComponent<UIButtons>().endlist = false;
            swprev.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        if (typecards == 0)
        {
            swnext.GetComponent<Image>().color = new Color(255, 255, 255);
            swnext.GetComponent<UIButtons>().endlist = false;
            swprev.GetComponent<UIButtons>().endlist = true;
            swprev.GetComponent<Image>().color = swcolor;
        }
        

        ChangeText(typecards);
    }

    public void Next(bool next)
    {
        if(next && typecards != cards.Length - 1)
        {
            typecards += 1;
            LoadPreview(true);
            //ChangeText(typecards);
        }
        if(!next && typecards != 0)
        {
            typecards -= 1;
            LoadPreview(true);
            //ChangeText(typecards);
        }
    }
}
