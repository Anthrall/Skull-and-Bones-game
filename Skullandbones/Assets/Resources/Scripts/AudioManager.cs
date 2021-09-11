using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    public GameObject Data;
    public int issound;
    public static AudioManager instance;

    void Start()
    {
        if (PlayerPrefs.HasKey("SoundOn"))
            issound = PlayerPrefs.GetInt("SoundOn");
        else
        {
            PlayerPrefs.SetInt("SoundOn", 1);
            issound = 1;
        }

        if (issound == 1)
        {
            Play("Theme");
            Play("Effect");
        }
        //PlayBackground();
    }


    void Awake()
    {
        int type = PlayerPrefs.GetInt("SetOfCards");
        issound = PlayerPrefs.GetInt("SoundOn");
        var theme = sounds[0];
        //int type = 1;
        theme.clip = Data.GetComponent<Data>().DataCrads[type].background_music;
        sounds[1].clip = Data.GetComponent<Data>().DataCrads[type].sound_effect;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        

         foreach (Sounds s in sounds)
         {
                 s.source = gameObject.AddComponent<AudioSource>();
                 s.source.clip = s.clip;

                 s.source.volume = s.volume;
                 s.source.pitch = s.pitch;
                 s.source.loop = s.loop;
         }

    }



    public void Stop(string name)
    {
        foreach (Sounds s in sounds)
        {
            if (s.source.isPlaying)
            {
                if (s.name == name)
                {
                    s.source.Stop();
                    break;
                }
            }
        }
    }

    public void Play(string name)
    {
        foreach(Sounds s in sounds)
        {
            if(s.name == name)
            {
                if (!s.source.isPlaying)
                {
                    if (s.source.loop)
                    {
                        s.source.Play();
                    }
                    else
                    {
                        s.source.PlayOneShot(s.source.clip);
                    }
                }
                break;
            }
        }
    }
}
