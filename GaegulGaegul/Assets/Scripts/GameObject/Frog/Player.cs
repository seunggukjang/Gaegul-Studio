using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]private Transform spawnTransform;
    [SerializeField]private Dissolve dissolve;
    
    Vector2 spawnPosition;
    private LayerMask deadThings;
    private LayerMask flag;
    private Grab grab;
    private Vector3 halfSize;
    private DeathCounter deathCounter;
    [SerializeField] private Animator animator;
    [SerializeField] private int skin = -1;
    private bool isDead = false;
    void Start()
    {
        deathCounter = GameObject.Find("DeathCounter").GetComponent<DeathCounter>();
        spawnTransform = GameObject.Find("StartPosition").transform;
        if (spawnTransform)
            spawnPosition = spawnTransform.position;
        else
            spawnPosition = transform.position;
        grab = GetComponent<Grab>();
        deadThings = 1 << LayerMask.NameToLayer("Dead") | 1 << LayerMask.NameToLayer("Enemy");
        flag = 1 << LayerMask.NameToLayer("Flag");
        halfSize = transform.lossyScale * 0.5f;

        // random skin color [temp ?]
        if (skin == -1)
             skin = Random.Range(0, 5);
        animator.SetLayerWeight(skin, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D deadLineCollide = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, deadThings);
        
        if(deadLineCollide)
        {
            StartCoroutine(Dead());
        }
            
            
    }
    IEnumerator Dead()
    {
        if (!isDead)
        {
            isDead = true;
            if (dissolve)
            {
                dissolve.SetIsDissolving(true);
                yield return new WaitForSeconds(2);
            }
            deathCounter.IncrementDeathCount();
            grab.CancelAllPulling();
            transform.position = spawnPosition;
            isDead = false;
        }
    }
    
}