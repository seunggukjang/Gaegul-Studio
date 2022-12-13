using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Transform range;
    [SerializeField] private LayerMask frogLayer;
    
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform stopTransform;
    [SerializeField] private float smoothTime = 0.99f;
    private AudioManager audioManager;
    private bool isMove = true;
    private Vector3 stopPosition;
    private Vector3 startPosition;
    private Vector3 velocity = Vector3.zero;
    private bool canMove = false;
    public bool isGrab = false;
    private int form_number;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        form_number = Random.Range(0, 2);
        ChangeSprite(form_number);
        if (startTransform && stopTransform)
        {
            canMove = true;
            stopPosition = stopTransform.position;
            startPosition = startTransform.position;
        }
            


    }
    private void ChangeSprite(int num)
    {
        SpriteRenderer spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        switch (num)
        {
            case 0:
                spriteRenderer.sprite = Resources.Load<Sprite>("Assets/Sprites/items/EnemyBee.png");
                break;
            case 1:
                spriteRenderer.sprite = Resources.Load<Sprite>("Assets/Sprites/items/EnemyBeetle.png");
                break;
            case 2:
                spriteRenderer.sprite = Resources.Load<Sprite>("Assets/Sprites/items/EnemyLadybug.png");
                break;
        }
    }
    void FixedUpdate()
    {
        Collider2D frogHit2D = Physics2D.OverlapCircle(transform.position, range.lossyScale.x * 0.5f + 0.2f, frogLayer);
        if(frogHit2D != null)
        {
            Grab grab = frogHit2D.transform.GetComponent<Grab>();
            if(grab.GetTargeID() == gameObject.GetInstanceID())
            {
                grab.CancelPulling();
            }
            Combat combat = frogHit2D.transform.GetComponent<Combat>();
            if(combat)
                combat.ChangeToForm(form_number);
            if (audioManager != null)
                audioManager.Play("coin");
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!canMove && !isGrab)
            return;
        if (isMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, stopPosition, ref velocity, smoothTime);
            if ((transform.position - stopPosition).sqrMagnitude < 0.1)
            {
                isMove = false;
            }
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothTime);
            if ((transform.position - startPosition).sqrMagnitude < 0.1)
            {
                isMove = true;
            }
        }
    }
}
