using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]private Transform spawnTransform;
    [SerializeField]private Dissolve dissolve;
    [SerializeField]private BlackHole blackHole;
    Vector2 spawnPosition;
    private LayerMask deadThings;
    private LayerMask saw;
    private LayerMask flag;
    private Grab grab;
    private Vector3 halfSize;
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
        animator.SetLayerWeight(skin, 1);
        deathCounter = GameObject.Find("DeathCounter").GetComponent<DeathCounter>();
        
        if (spawnTransform)
            spawnPosition = spawnTransform.position;
        else
            spawnPosition = transform.position;
        grab = GetComponent<Grab>();
        deadThings = 1 << LayerMask.NameToLayer("Dead") | 1 << LayerMask.NameToLayer("Enemy");
        flag = 1 << LayerMask.NameToLayer("Flag");
        saw = 1 << LayerMask.NameToLayer("Saw");
        halfSize = transform.lossyScale * 0.5f;
        audioManager = AudioManager.instance;
        if (!audioManager)
            audioManager = FindObjectOfType<AudioManager>();
    }
    void FixedUpdate()
    {
        Collider2D deadLineCollide = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, deadThings);
        Collider2D sawCollider = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, saw);

        if (deadLineCollide)
        {
            StartCoroutine(Dead("deadline"));
        }    
        else if(sawCollider)
        {
            StartCoroutine(Dead("saw"));
        }
    }
    public void ReviveSkin()
    {
        animator.SetLayerWeight(skin, 1);
    }
    public void ChangeSkin(int skin)
    {
        int skinnbr = 8;
        for (int i = 0; i < skinnbr; i++) {
            animator.SetLayerWeight(i, 0);

        }
        animator.SetLayerWeight(skin, 1);
    }

    IEnumerator Revive(string whatDead)
    {
        if (dissolve && whatDead == "deadline")
        {
            StopAllCoroutines();
            dissolve.SetIsAppear(true);
            yield return new WaitForSeconds(2);
        }
        else if (blackHole && whatDead == "saw")
        {
            StopAllCoroutines();
            blackHole.SetIsAppear(true);
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator Dead(string whatDead)
    {
        if (!isDead)
        {
            isDead = true;
            if (whatDead == "deadline")
            {
                if (audioManager)
                {
                    audioManager.Play("dead");
                }
                if (dissolve)
                {
                    dissolve.SetIsDisappear(true);
                    yield return new WaitForSeconds(2);
                }
            }
            else if(whatDead == "saw")
            {
                if (audioManager)
                {
                    audioManager.Play("deadsaw");
                }
                if (blackHole)
                {
                    blackHole.SetIsTwirl(true);
                    yield return new WaitForSeconds(2);
                }
            }
            deathCounter.IncrementDeathCount();
            grab.CancelAllPulling();
            transform.position = spawnPosition;
            isDead = false;
            
            StartCoroutine(Revive(whatDead));

        }
    }

    public int GetSkin()
    {
        return (skin);
    }
    
}
