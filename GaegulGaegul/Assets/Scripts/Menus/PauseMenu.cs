using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private int menuIndex;
    [SerializeField] private GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                pauseMenu.SetActive(false);
            else
                pauseMenu.SetActive(true);
        }
    }

    public void ExitPauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}