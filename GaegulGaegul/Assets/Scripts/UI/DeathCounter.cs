using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private int deathCount = 0;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();        
    }

    public void IncrementDeathCount()
    {
        deathCount++;
        textMeshPro.text = "Death(s) : " + deathCount.ToString();
    }

    public int getDeathCount()
    {
        return deathCount;
    }
}