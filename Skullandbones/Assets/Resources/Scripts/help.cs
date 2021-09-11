using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class help : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject help_desk, gameplay;
    public Color color_enter;

    void Start()
    {
        //var g = gameplay.GetComponent<gameplay>();
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var g = gameplay.GetComponent<gameplay>();
        g.query = true;
        this.gameObject.GetComponent<Image>().color = color_enter;
        help_desk.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var g = gameplay.GetComponent<gameplay>();
        g.query = false;
        this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
        help_desk.SetActive(false);
    }

}
