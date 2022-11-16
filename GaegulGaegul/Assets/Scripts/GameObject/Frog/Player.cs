using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        deathCounter = GameObject.Find("DeathCounter").GetComponent<DeathCounter>();
        spawnPosition = spawnTransform.position;
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
        Collider2D endFlag = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, flag);
        if(deadLineCollide)
        {
            StartCoroutine(Dead());
        }
            
        if(endFlag)
        {
            StartCoroutine(NextScene());
        }
            
    }
    IEnumerator Dead()
    {
        dissolve.SetIsDissolving(true);
        yield return new WaitForSeconds(2);
        deathCounter.IncrementDeathCount();
        grab.CancelAllPulling();
        transform.position = spawnPosition;
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
