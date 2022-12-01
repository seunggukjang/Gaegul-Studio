using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    int grabNumber;
    public void AddGrabNumber(int number)
    {
        grabNumber += number;
    }
    public void IncreaseGrabNumber()
    {
        grabNumber++;
    }
    public void DecreaseGrabNumber()
    {
        grabNumber--;
    }
    public void SetGrabNumber(int number)
    {
        grabNumber = number;
    }
    public int GetGrabNumber()
    {
        return grabNumber;
    }
}
