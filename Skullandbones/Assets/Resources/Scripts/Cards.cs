using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cards
{
    public string name;
    public string name_ru;
    public Sprite logo, shirt;
    public Faces[] faces;
    public Color[] cards_color;
    public Color player_text_color, computer_text_color, background_color, board_color;
    public GameObject particle;
    public AudioClip background_music, sound_effect;
    public int[] num_of_choose;
}

[System.Serializable]
public class Faces
{
    public string name;
    public string name_ru;
    public Sprite sprite;
}
