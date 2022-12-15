using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] private bool isPvPmode = false;
    private bool isAlive = true;
    [SerializeField] private float respawnTime = 12f;
    [SerializeField] private Transform startPosition;
    private float timer = 0;
    [SerializeField] GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        timer = respawnTime;
    }
    public void SetTimerZero()
    {
        timer = 0;
    }
    bool isFirst = true;
    // Update is called once per frame
    void Update()
    {
        if (!isPvPmode)
            return;
        if(item.activeSelf == false)
        {
            if(isFirst)
            {
                isFirst = false;
                timer = 0f;
            }
            if(timer < respawnTime)
            {
                timer += Time.deltaTime;
            }
            else if(timer >= respawnTime)
            {
                timer = respawnTime;
                item.SetActive(true);
                item.GetComponent<Item>().moveNumber = 0;
                item.GetComponent<Item>().form_number = Random.Range(0, 10) % 3;
                item.transform.position = startPosition.position;
                isFirst = true;
            }
        }
    }
}
