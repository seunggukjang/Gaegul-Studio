using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public int damageTaken = 0;

    void Start()
    {
        damageTaken = 0;
    }

    void TakeDamage(int amount)
    {
        damageTaken += amount;
    }
}
