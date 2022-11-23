using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Trigger : MonoBehaviour
{
    abstract public bool GetIsWork();
    protected bool isWork = false;
}
