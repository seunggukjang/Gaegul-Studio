using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Sprite[] iconList;
    public Sprite   iconDead;
    
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetDead()
    {
        image.sprite = iconDead;
    }

    public void SetIcon(int index)
    {
        image.sprite = iconList[index];
    }
}
