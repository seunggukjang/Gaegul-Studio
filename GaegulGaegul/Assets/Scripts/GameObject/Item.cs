using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Transform range;
    [SerializeField] private LayerMask frogLayer;
    
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform stopTransform;
    [SerializeField] private Transform middleTransform1;
    [SerializeField] private Transform middleTransform2;
    [SerializeField] private float smoothTime = 1.2f;

    private AudioManager audioManager;
    private bool isMove = true;
    private Vector3 stopPosition;
    private Vector3 startPosition;
    private Vector3 middlePosition1;
    private Vector3 middlePosition2;
    private Vector3 velocity = Vector3.zero;
    private bool canMove = false;
    private bool camMove2 = false;
    public bool isGrab = false;
    public int moveNumber = 0;
    public int form_number;
    
    private void Start()
    {
        audioManager = AudioManager.instance;
        if (!audioManager)
            audioManager = FindObjectOfType<AudioManager>();
        form_number = Random.Range(0, 10);
        ChangeSprite(form_number);
        if (startTransform && stopTransform)
        {
            canMove = true;
            stopPosition = stopTransform.position;
            startPosition = startTransform.position;
        }
        if(middleTransform1 && middleTransform2)
        {
            camMove2 = true;
            middlePosition1 = middleTransform1.position;
            middlePosition2 = middleTransform2.position;
        }


    }
    private void ChangeSprite(int num)
    {
        SpriteRenderer spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        switch (num % 3)
        {
            case 0:
                spriteRenderer.sprite = Resources.Load<Sprite>("EnemyBee");
                break;
            case 1:
                spriteRenderer.sprite = Resources.Load<Sprite>("EnemyLadybug");
                break;
            case 2:
                spriteRenderer.sprite = Resources.Load<Sprite>("EnemyBeetle");
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
        if(camMove2)
        {
            if(moveNumber == 0)
            {
                transform.position = Vector3.SmoothDamp(transform.position, middlePosition1, ref velocity, smoothTime);
                if ((transform.position - middlePosition1).sqrMagnitude < 0.1)
                {
                    moveNumber = 1;
                }
            }
            else if(moveNumber == 1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, middlePosition2, ref velocity, smoothTime);
                if ((transform.position - middlePosition2).sqrMagnitude < 0.1)
                {
                    moveNumber = 2;
                }
            }
            else if (moveNumber == 2)
            {
                transform.position = Vector3.SmoothDamp(transform.position, stopPosition, ref velocity, smoothTime);
                if ((transform.position - stopPosition).sqrMagnitude < 0.1)
                {
                    moveNumber = 3;
                }
            }
            else if (moveNumber == 3)
            {
                transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothTime);
                if ((transform.position - middlePosition2).sqrMagnitude < 0.1)
                {
                    moveNumber = 0;
                }
            }
        }
        else {
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
}
