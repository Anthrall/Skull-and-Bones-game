using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Color color_enter, color_click;
    
    public bool endlist;

    void Start()
    {
        
        
    }


    public void OpenSetting()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        FindObjectOfType<AudioManager>().Stop("Win");
        FindObjectOfType<AudioManager>().Stop("Lose");
        FindObjectOfType<AudioManager>().Stop("NeutralEnd");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenBehance()
    {
        Application.OpenURL("https://www.behance.net/anthrall");
    }

    public void OpenVK()
    {
        Application.OpenURL("https://vk.com/nazadvorkach");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!endlist)
            this.gameObject.GetComponent<Image>().color = color_click;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!endlist)
            this.gameObject.GetComponent<Image>().color = color_enter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!endlist)
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!endlist)
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
    }
}
