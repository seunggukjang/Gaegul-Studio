using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    Vector3 mousePosition = new Vector3();
    void Update()
    {
        if(Camera.main)
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public Vector3 GetMousePosition()
    {
        return mousePosition;
    }
}