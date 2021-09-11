using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class card_settings : MonoBehaviour
{
    public string card_name;
    private string lang;
    public int id_type, id_card, id;
    public GameObject shirt, controller;
    public Vector3 pos_on_board, scale;
    private Vector3 normal, enter, shirt_pos;
    public Sprite normal_board, enter_board;
    public Text discription;
    public int collect_type; //0 - не собрано // 1 - у игрока // 2 - у компа
    public bool collect;

    private void Start()
    {
        lang = PlayerPrefs.GetString("Language");
        normal = transform.localScale;
        enter = new Vector3(normal.x + 0.05f, normal.y + 0.05f, normal.z);
        shirt_pos = new Vector3(shirt.transform.position.x, shirt.transform.position.y, -6);
    }

    public void OnMouseEnter()
    {
        //Debug.Log("enter "+id);
        if(collect_type == 0 && !controller.GetComponent<gameplay>().query && transform.position != shirt_pos)
        {
            //Debug.Log("enter1 " + id);
            var Seq = DOTween.Sequence();
            Seq.Append(this.gameObject.transform.DOScale(enter, 0.2f));
            Seq.Join(this.gameObject.transform.DOMoveZ(-7f, 0.2f));

            if(lang=="RUS")
                discription.text = "Карта \""+card_name+"\"";
            else
                discription.text = "\"" + card_name + "\" card";

            this.gameObject.GetComponent<SpriteRenderer>().sprite = enter_board;
        }
        if(controller.GetComponent<gameplay>().query)
        {
            this.gameObject.transform.DOScale(normal, 0.05f);
        }
    }

    public void OnMouseExit()
    {
       // Debug.Log("exit " + id);
        if (collect_type == 0 && !controller.GetComponent<gameplay>().query && transform.position != shirt_pos)
        {
           // Debug.Log("exit1 " + id);
            var Seq = DOTween.Sequence();
            Seq.Append(this.gameObject.transform.DOScale(normal, 0.2f));
            Seq.Join(this.gameObject.transform.DOMoveZ(-6f, 0.2f));
            discription.text = "";
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normal_board;
        }
        if (controller.GetComponent<gameplay>().query)
        {
            this.gameObject.transform.DOScale(normal, 0.05f);
        }
    }

    public void OnMouseDown()
    {
        if (collect_type == 0 && !controller.GetComponent<gameplay>().query && transform.position != shirt_pos)
        {
            controller.GetComponent<gameplay>().query = true;
            //Debug.Log("Click");
            var Seq = DOTween.Sequence();
            var obj = this.gameObject.transform;
            Seq.Append(obj.DOScale(new Vector3(normal.x - 0.02f, normal.y - 0.02f, normal.z), 0.1f));
            Seq.Join(obj.DOMoveZ(-7f, 0.1f));
            Seq.AppendInterval(0.05f);
            Seq.Append(obj.DOScale(normal, 0.1f));
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normal_board;
            Seq.Join(obj.transform.DOMoveZ(-6f, 0.1f));
            controller.GetComponent<gameplay>().QueryCard(id_type, true);
        }
        
    }
}
