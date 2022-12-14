using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToScene : MonoBehaviour
{
    AudioManager audio;
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        
    }
    public void LoadwithSceneID(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
    void ChangeBGM(int sceneID)
    {
        switch(sceneID)
        {

        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
