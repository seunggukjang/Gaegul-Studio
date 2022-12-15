using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance = new AudioManager();
    private string current_sound_name;
    public static int previous_sceneID = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.vloume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        Play("menubgm");
    }
    public string GetCurrentPlay()
    {
        return current_sound_name;
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        current_sound_name = name;
        s.source.Play();
    }
    public void SetVolume(float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == current_sound_name);
        s.source.volume = volume;
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Stop();
    }
    public void ChangeBGM(int sceneID)
    {
        if (sceneID == 3)
        {
            previous_sceneID = sceneID;
            return;
        }
        else if (sceneID <= 5)
        {
            if (previous_sceneID > 6)
            {
                Play("menubgm");
                Stop("jungle");
                Stop("bgm");
            }
        }
        else if (sceneID <= 8)
        {
            if (previous_sceneID > 9 || previous_sceneID < 6)
            {
                Stop("menubgm");
                Stop("jungle");
                Play("bgm");
            }
        }
        else if (sceneID <= 13)
        {
            if (previous_sceneID >= 0 || previous_sceneID <= 9)
            {
                Stop("menubgm");
                Play("jungle");
                Stop("bgm");
            }
        }

        previous_sceneID = sceneID;
    }
    public void ChangeBGM(string levelname)
    {
        if (levelname == "Tuto 1" || levelname == "Tuto 2" || levelname == "Tuto 3")
        {
            Stop("menubgm");
            Stop("jungle");
            Play("bgm");
        }
        else if (levelname == "pvpMap1")
        {
            Stop("menubgm");
            Play("jungle");
            Stop("bgm");
        }

    }
}
