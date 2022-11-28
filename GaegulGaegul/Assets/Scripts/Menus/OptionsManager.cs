using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : really disable the sounds and music

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject soundEffectDisabled;
    [SerializeField] private GameObject musicButtonDisabled;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void soundManager()
    {
        if (PlayerPrefs.GetInt("SoundEffects") == 1)
        {
            PlayerPrefs.SetInt("SoundEffects", 0);
            soundEffectDisabled.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("SoundEffects", 1);
            soundEffectDisabled.SetActive(false);
        }
    }

    public void musicManager()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
            musicButtonDisabled.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
            musicButtonDisabled.SetActive(false);
        }
    }
}
