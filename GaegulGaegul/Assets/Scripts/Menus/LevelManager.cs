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
}

public class LevelManager : MonoBehaviour
{
    public List<Level> LevelList;
    public GameObject LevelButtonPrefab;
    public Transform Spacer;
    public Transform Canvas;

    [Space]
    public GameObject NextPage;
    public GameObject PreviousPage;
    public int currentDisplayedPage = 0;
    private int totalPages = 0;
    private Spacer[] pages;

    private AudioManager audio;
    void Start()
    {
        fillList();
        handlePages();
        audio = AudioManager.instance;
        //SaveAll();
    }

    void fillList()
    {
        int levelNumber = 0;
        Transform firstPage = Instantiate(Spacer) as Transform;
        firstPage.transform.SetParent(Canvas, false);
        Transform currentPage = firstPage;
    
        foreach (var level in LevelList)
        {
            if (levelNumber % 9 == 0 && levelNumber != 0)
            {
                Transform newPage = Instantiate(Spacer) as Transform;
                newPage.transform.SetParent(Canvas, false);
                currentPage = newPage;
            }
            GameObject newButton = Instantiate(LevelButtonPrefab) as GameObject;
            LevelButton button = newButton.GetComponent<LevelButton>();

            button.levelNumber.GetComponent<Text>().text = level.levelNumber.ToString();
            button.levelNumberInt = level.levelNumber;
            button.levelName = level.levelName;
            button.levelCompleted = false;
            button.GetComponentInChildren<Button>().interactable = true;
            button.GetComponentInChildren<Button>().onClick.AddListener(
                () => loadLevel(level.levelName)
            );
            newButton.transform.SetParent(currentPage, false);
            levelNumber++;
        }
    }

    void handlePages()
    {
        pages = Canvas.GetComponentsInChildren<Spacer>();
        totalPages = pages.Length;
        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }
        pages[currentDisplayedPage].gameObject.SetActive(true);
    }

    void Update() 
    {
        if (totalPages == 1) {
            NextPage.SetActive(false);
            PreviousPage.SetActive(false);
            return;
        }

        if (currentDisplayedPage == 0) {
            PreviousPage.SetActive(false);
            NextPage.SetActive(true);
        }
        else if (currentDisplayedPage == totalPages - 1) {
            PreviousPage.SetActive(true);
            NextPage.SetActive(false);
        }
        else {
            PreviousPage.SetActive(true);
            NextPage.SetActive(true);
        }
    }

    public void nexpPage()
    {
        pages[currentDisplayedPage].gameObject.SetActive(false);
        currentDisplayedPage++;
        pages[currentDisplayedPage].gameObject.SetActive(true);
    }

    public void previousPage()
    {
        pages[currentDisplayedPage].gameObject.SetActive(false);
        currentDisplayedPage--;
        pages[currentDisplayedPage].gameObject.SetActive(true);
    }

    void loadLevel(string levelName)
    {
        if (audio)
        {
            audio.ChangeBGM(levelName);
            Debug.Log("levelname changed");
        }
            
        SceneManager.LoadScene(levelName);
    }

    /*void SaveAll()
    {
        GameObject[] allButtons = GameObject.FindGameObjectsWithTag("LevelButton");

        foreach (var button in allButtons)
        {
            LevelButton levelButton = button.GetComponent<LevelButton>();
            Level level = LevelList[levelButton.levelNumberInt - 1];

            if (level.levelCompleted)
                PlayerPrefs.SetInt("LevelCompleted" + level.levelNumber, 1);
            else
                PlayerPrefs.SetInt("LevelCompleted" + level.levelNumber, 0);
        }
    }*/
}
