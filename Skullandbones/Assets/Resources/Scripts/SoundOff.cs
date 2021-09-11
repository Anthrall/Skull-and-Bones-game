using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundOff : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Color color_enter, color_click;
    private int issound;
    public Sprite sound_on, sound_off;

    // Start is called before the first frame update
    void Start()
    {
        issound = PlayerPrefs.GetInt("SoundOn");
        Awake();
    }

    void Awake()
    {
        issound = PlayerPrefs.GetInt("SoundOn");
        if(issound == 1)
        {
            this.gameObject.GetComponent<Image>().sprite = sound_on;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = sound_off;
        }
    }

    public void SoundChange()
    {

        if (issound == 1)
        {
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Stop("Effect");
            issound = 0;
            PlayerPrefs.SetInt("SoundOn", issound);
            this.gameObject.GetComponent<Image>().sprite = sound_off;
        }
        else
        {
            issound = 1;
            PlayerPrefs.SetInt("SoundOn", issound);
            FindObjectOfType<AudioManager>().Play("Theme");
            FindObjectOfType<AudioManager>().Play("Effect");
            this.gameObject.GetComponent<Image>().sprite = sound_on;
        }
        //issound = PlayerPrefs.GetInt("SoundOn");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
            this.gameObject.GetComponent<Image>().color = color_click;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
            this.gameObject.GetComponent<Image>().color = color_enter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
    }
}
