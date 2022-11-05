using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private int HealthPoint = 3;
    public void Damage(int damagePoint)
    {
        HealthPoint -= damagePoint;
    }
    void Update()
    {
        if(HealthPoint <= 0)
            gameObject.SetActive(false);
    }
}
