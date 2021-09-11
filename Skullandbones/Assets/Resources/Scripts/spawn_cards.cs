using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class spawn_cards : MonoBehaviour
{
    [HideInInspector]
    public Cards[] cards;
    private string lang;
    public GameObject DataCards;
    public int typecards;
    private int id_card;
    public GameObject maincamera, controller, card, allcards, shirt, logo, background, board_top, board_down, text_f_t, text_f_d;
    public Text text;
    public float start_y, start_y2, step_x;
    private float start_x;
    public Color[] card_color;
    public float scale;
    //private string hex;
    // Start is called before the first frame update
    void Start()
    {
        var gameplay = controller.GetComponent<gameplay>();
        gameplay.query = true;
        cards = DataCards.GetComponent<Data>().DataCrads;
        //Debug.Log(ColorUtility.ToHtmlStringRGBA(card_color[0]));
        if (PlayerPrefs.HasKey("SetOfCards"))
            typecards = PlayerPrefs.GetInt("SetOfCards");
        else
        {
            PlayerPrefs.SetInt("SetOfCards", 0);
            typecards = 0;
        }
        //typecards = 1;
        card_color = cards[typecards].cards_color;

        lang = PlayerPrefs.GetString("Language");
        Spawn();
    }

    public void Spawn()
    {
        var gameplay = controller.GetComponent<gameplay>();
        //gameplay.query = true;
        gameplay.player_board = new int[10,3];
        gameplay.computer_board = new int[10,3];
        gameplay.deck = new List<int>();
        start_x = 0 - (step_x * ((gameplay.player_board.GetLength(0) * gameplay.player_board.GetLength(1)) / 4 - 0.5f) + 0.57f);
        float orig_x = start_x;
        Vector3 newpos;
        var cardsdata = cards[typecards];
        id_card = 0;

        background.GetComponent<SpriteRenderer>().color = cardsdata.background_color;
        board_top.GetComponent<SpriteRenderer>().color = cardsdata.board_color;
        board_down.GetComponent<SpriteRenderer>().color = cardsdata.board_color;
        text_f_t.GetComponent<Image>().color = cardsdata.background_color;
        text_f_d.GetComponent<Image>().color = cardsdata.background_color;

        logo.GetComponent<SpriteRenderer>().sprite = cardsdata.logo;
        shirt.GetComponent<SpriteRenderer>().sprite = cardsdata.shirt;
        gameplay.shirt_sprite = cardsdata.shirt;

        gameplay.player_answer = cards[typecards].player_text_color;
        gameplay.computer_answer = cards[typecards].computer_text_color;

        GameObject particle = Instantiate(cardsdata.particle, new Vector3(0,0,-8.5f), Quaternion.identity) as GameObject;
        particle.transform.SetParent(maincamera.transform);
        particle.transform.rotation = cardsdata.particle.transform.rotation;
        particle.GetComponent<ParticleSystem>().Play();

        for (int i=0; i < gameplay.player_board.GetLength(0); i++)
        {
            for (int j=0; j< gameplay.player_board.GetLength(1); j++)
            {
                if (i < gameplay.player_board.GetLength(0) / 2)
                {
                    newpos = new Vector3(start_x + (j * step_x), start_y, -6f);
                }
                else
                {
                    newpos = new Vector3(start_x + (j * step_x), start_y2, -6f);
                    
                }

                GameObject newCard = Instantiate(card, newpos, Quaternion.identity) as GameObject;
                newCard.transform.SetParent(allcards.transform);
                newCard.transform.localScale = new Vector3(scale, scale, scale);
                newCard.GetComponent<card_settings>().id_type = i;
                newCard.GetComponent<card_settings>().id_card = j;
                newCard.GetComponent<card_settings>().id = id_card;

                if (lang == "RUS")
                    newCard.GetComponent<card_settings>().card_name = cardsdata.faces[i].name_ru;
                else
                    newCard.GetComponent<card_settings>().card_name = cardsdata.faces[i].name;


                newCard.GetComponent<card_settings>().pos_on_board = newCard.transform.position;
                newCard.GetComponent<card_settings>().scale = new Vector3(scale, scale, scale);
                newCard.GetComponent<card_settings>().shirt = shirt;
                newCard.GetComponent<card_settings>().discription = text;
                newCard.GetComponent<card_settings>().controller = controller;
                newCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardsdata.faces[i].sprite;
                newCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = card_color[j];
                newCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = card_color[j];
                newCard.name = "Card" + i + "_" + j + "_" +id_card;
                newCard.transform.position = new Vector3(shirt.transform.position.x, shirt.transform.position.y, -6f);
                newCard.transform.rotation = Quaternion.Euler(shirt.transform.rotation.eulerAngles);
                newCard.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
                newCard.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
                //newCard.SetActive(false);

                gameplay.deck.Add(id_card++);
                
            }
            if (i == (gameplay.player_board.GetLength(0) / 2)-1)
                start_x = orig_x;
            else
                start_x = start_x + (gameplay.player_board.GetLength(1) * step_x);
        }
        gameplay.gameready = true;
        
        gameplay.StartGame();


    }
}
