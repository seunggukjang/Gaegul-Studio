using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundContoller : MonoBehaviour
{
    AudioManager audio;
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        if(SceneManager.GetActiveScene().name == "pvpMap1")
        {
            audio.Stop("bgm");
            audio.Play("jungle");
            audio.Play("battle");
            audio.Play("battle2");
        }
        else 
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
