using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UITimer : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        textMeshPro.text = "Timer : " + timer.ToString("F1");
    }
}
