using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLoad : MonoBehaviour
{

    [HideInInspector]
    public Cards[] cards;

    public GameObject DataCards, background, maincamera;
    public GameObject[] buttons;
    public int typecards;
    public Text version;
    // Start is called before the first frame update
    void Start()
    {
        cards = DataCards.GetComponent<Data>().DataCrads;
        if (PlayerPrefs.HasKey("SetOfCards"))
            typecards = PlayerPrefs.GetInt("SetOfCards");
        else
        {
            PlayerPrefs.SetInt("SetOfCards", 0);
            typecards = 0;
        }
        version.text = "v. " + Application.version;
        LoadSetting();
    }

    public void LoadSetting()
    {
        var cardsdata = cards[typecards];
        background.GetComponent<SpriteRenderer>().color = cardsdata.background_color;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.GetComponent<Image>().color = cardsdata.background_color;
            //buttons[i].transform.GetChild(0).GetComponent<Text>().color = cardsdata.player_text_color;
        }

        GameObject particle = Instantiate(cardsdata.particle, new Vector3(0, 0, -8.5f), Quaternion.identity) as GameObject;
        particle.transform.SetParent(maincamera.transform);
        particle.transform.rotation = cardsdata.particle.transform.rotation;
        particle.GetComponent<ParticleSystem>().Play();
    }
}
