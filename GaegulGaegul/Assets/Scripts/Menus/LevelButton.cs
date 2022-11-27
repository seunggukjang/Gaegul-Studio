using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string levelName;
    public Text levelNumber;
    public int levelNumberInt;
    public bool levelUnlocked;

    public bool levelCompleted;
    public GameObject star;
}
