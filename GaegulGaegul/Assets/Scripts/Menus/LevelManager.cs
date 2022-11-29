using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public string levelName;
    public int levelNumber;
    public bool levelCompleted;
    public bool levelUnlocked;
}

public class LevelManager : MonoBehaviour
{
    public List<Level> LevelList;
    public GameObject LevelButtonPrefab;
    public Transform Spacer;

    void Start()
    {
        fillList();
        //SaveAll();
    }

    void fillList()
    {
        foreach (var level in LevelList)
        {
            GameObject newButton = Instantiate(LevelButtonPrefab) as GameObject;
            LevelButton button = newButton.GetComponent<LevelButton>();

            button.levelNumber.GetComponent<Text>().text = level.levelNumber.ToString();
            button.levelNumberInt = level.levelNumber;
            button.levelName = level.levelName;
            //if (PlayerPrefs.GetInt("LevelCompleted" + level.levelNumber.ToString()) == 1) {
                button.levelCompleted = true;
                button.levelUnlocked = true;
            //}
            /*if (level.levelNumber == 1) // unlock first level
                button.levelUnlocked = true;*/

            button.GetComponentInChildren<Button>().interactable = button.levelUnlocked;
            button.GetComponentInChildren<Button>().onClick.AddListener(
                () => loadLevel(level.levelName)
            );
            newButton.transform.SetParent(Spacer, false);
        }
    }

    void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    void SaveAll()
    {
        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");

        foreach (var button in allButtons)
        {
            LevelButton levelButton = button.GetComponent<LevelButton>();
            Level level = LevelList[levelButton.levelNumberInt - 1];

            if (level.levelCompleted) {
                PlayerPrefs.SetInt("LevelCompleted" + level.levelNumber, 1);
                PlayerPrefs.SetInt("LevelUnlocked" + level.levelNumber, 1);
            }
            else {
                PlayerPrefs.SetInt("LevelCompleted" + level.levelNumber, 0);
                PlayerPrefs.SetInt("LevelUnlocked" + level.levelNumber, 0);
            }
        }
    }
}
