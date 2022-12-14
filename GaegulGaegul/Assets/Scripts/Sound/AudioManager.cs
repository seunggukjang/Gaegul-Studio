using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public string current_sound_name;
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
        if (SceneManager.GetActiveScene().name == "pvpMap1")
        {
            GetComponent<AudioSource>().Stop("bgm");
            GetComponent<AudioSource>().Play("jungle");
            GetComponent<AudioSource>().Play("battle");
            GetComponent<AudioSource>().Play("battle2");
        }
        else
        {

        }
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
}
