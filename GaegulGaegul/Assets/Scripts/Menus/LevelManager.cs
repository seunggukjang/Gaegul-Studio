using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    void fillList()
    {
        foreach (var level in LevelList)
        {
            GameObject newButton = Instantiate(LevelButtonPrefab) as GameObject;
            LevelButton button = newButton.GetComponent<LevelButton>();
            button.levelCompleted = level.levelCompleted;
            button.levelUnlocked = level.levelUnlocked;
            button.levelNumber.GetComponent<Text>().text = level.levelNumber.ToString();
            newButton.transform.SetParent(Spacer, false);
        }
    }
}
