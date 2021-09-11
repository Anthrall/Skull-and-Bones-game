using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MeinMenuUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Sprite normal, enter, click, normal_gold;
    public bool isgold ;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(2);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isgold)
            this.gameObject.GetComponent<Image>().sprite = click;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isgold)
            this.gameObject.GetComponent<Image>().sprite = enter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isgold)
            this.gameObject.GetComponent<Image>().sprite = normal;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isgold)
            this.gameObject.GetComponent<Image>().sprite = enter;
    }
}
