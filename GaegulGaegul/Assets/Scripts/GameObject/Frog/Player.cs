using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]private Transform spawnTransform;
    [SerializeField]private Dissolve dissolve;
    [SerializeField]private BlackHole blackHole;
    [SerializeField] private Combat combat;
    private bool isPvPmode = false;
    Vector2 spawnPosition;
    private LayerMask deadThings;
    private LayerMask saw;
    private LayerMask flag;
    private Grab grab;
    private Vector3 halfSize;
    private bool isDisolveDead = false;
    private bool isBlackHoldeDead = false;
    private DeathCounter deathCounter;
    [SerializeField] private Animator animator;
    [SerializeField] private int skin = -1;
    private bool isDead = false;
    //private bool isRevive = false;
    private AudioManager audioManager;
    bool isChange = false;
    private void Awake()
    {
        // random skin color [temp ?]
        if (skin == -1)
            skin = Random.Range(0, 5);
        animator.SetLayerWeight(skin, 1);
    }
    
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
        saw = 1 << LayerMask.NameToLayer("Saw");
        flag = 1 << LayerMask.NameToLayer("Flag");
        halfSize = transform.lossyScale * 0.5f;
        audioManager = FindObjectOfType<AudioManager>();
        isDisolveDead = false;
        isBlackHoldeDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Collider2D deadLineCollide = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, deadThings);
        Collider2D deadSaw = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, saw);
        if (deadLineCollide)
        {
            isDisolveDead = true;
            StartCoroutine(Dead(false));
        }
        else if(deadSaw)
        {
            
            isBlackHoldeDead = true;
            StartCoroutine(Dead(true));
        }
            
    }
    IEnumerator Revive()
    {
        if (dissolve && isDisolveDead)
        {
            dissolve.SetIsAppear(true);
            isDisolveDead = false;
            yield return new WaitForSeconds(2);
        }
        else if(blackHole && isBlackHoldeDead)
        {
            blackHole.SetIsAppear(true);
            isBlackHoldeDead=false;
            yield return new WaitForSeconds(2);
        }
    }
    IEnumerator Dead(bool isSawDead)
    {
        if (!isDead)
        {
            isDead = true;
            if (audioManager)
            {
                if(isSawDead)
                {
                    audioManager.Play("deadsaw");
                }
                else
                {
                    audioManager.Play("dead");
                }
            }
                
            if (dissolve && isDisolveDead)
            {
                dissolve.SetIsDisappear(true);
                yield return new WaitForSeconds(2);
            }
            else if (blackHole && isBlackHoldeDead)
            {
                Debug.Log("Working22");
                blackHole.SetIsTwirl(true);
                yield return new WaitForSeconds(2);
            }
            deathCounter.IncrementDeathCount();
            grab.CancelAllPulling();
            transform.position = spawnPosition;
            isDead = false;
            
            StartCoroutine(Revive());

        }
    }
}
