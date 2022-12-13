using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrigger : Trigger
{
    [SerializeField] bool isFire = false;
    SpriteRenderer spriteRenderer;
    Vector2 halfSize;
    Vector2 position;
    LayerMask frogMask;
    [SerializeField] private Cannon canon;
    [SerializeField] private GameObject onSprite;
    [SerializeField] private GameObject offSprite;
    private AudioManager audioManager;
    bool isOff = false;
    void Start()
    {
        this.isWork = false;
        halfSize = transform.lossyScale * 0.5f;
        position = transform.position;
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
        frogMask = 1 << LayerMask.NameToLayer("Frog");
    }
    public override bool GetIsWork() { return this.isWork; }
    // Update is called once per frame
    private void FixedUpdate()
    {
        this.isWork = false;
        Collider2D collider = Physics2D.OverlapArea(position - halfSize, position + halfSize, frogMask);
        if(collider != null && !isOff)
        {
            onSprite.SetActive(false);
            offSprite.SetActive(true);
            isOff = true;
        }
        else if(collider == null && isOff)
        {
            onSprite.SetActive(true);
            offSprite.SetActive(false);
            isOff = false;
        }
        if (canon.GetFrogIn() && (collider || isFire))
        {
            this.isWork = true;
            canon.Fire();
            canon.SetFrogIn(false);
            
        }
    }
}
