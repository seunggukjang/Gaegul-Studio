using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class percentDamage : MonoBehaviour
{
    private TextMeshProUGUI Text;
    private float dmgTaken = 0;

    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Text.text = dmgTaken.ToString() + "%";
    }

    public void UpdateDmgTaken(float dmg)
    {
        dmgTaken = dmg;
    }
}
