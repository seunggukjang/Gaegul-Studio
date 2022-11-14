using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Transform spawnTransform;
    Vector2 spawnPosition;
    private LayerMask deadThings;
    private LayerMask flag;
    private Grab grab;
    private Vector3 halfSize;
    private DeathCounter deathCounter;

    void Start()
    {
        deathCounter = GameObject.Find("DeathCounter").GetComponent<DeathCounter>();
        spawnPosition = spawnTransform.position;
        grab = GetComponent<Grab>();
        deadThings = 1 << LayerMask.NameToLayer("Dead") | 1 << LayerMask.NameToLayer("Enemy");
        flag = 1 << LayerMask.NameToLayer("Flag");
        halfSize = transform.lossyScale * 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D deadLineCollide = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, deadThings);
        Collider2D endFlag = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, flag);
        if(deadLineCollide)
            Dead();
        if(endFlag)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Dead()
    {
        deathCounter.IncrementDeathCount();
        grab.CancelAllPulling();
        transform.position = spawnPosition;
    }
}
