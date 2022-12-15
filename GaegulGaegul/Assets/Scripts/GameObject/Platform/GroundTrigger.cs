using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : Trigger
{
    
    Vector2 halfSize;
    Vector2 position;
    LayerMask frogMask;
    [SerializeField] private GameObject offSprite;
    [SerializeField] private GameObject onSprite;
    private AudioManager audioManager;
    bool isOff = false;

    void Start()
    {
        audioManager = AudioManager.instance;
        this.isWork = false;
        halfSize = transform.lossyScale * 0.5f;
        position = transform.position;
        frogMask = 1 << LayerMask.NameToLayer("Frog");
    }
    public override bool GetIsWork() { return this.isWork; }
    private void FixedUpdate()
    {
        if(this.isWork)
            return;
        Collider2D collider = Physics2D.OverlapArea(position - halfSize, position + halfSize, frogMask);
        if (collider != null && !isOff)
        {
            onSprite.SetActive(false);
            offSprite.SetActive(true);
            if (audioManager)
                audioManager.Play("switch");
            this.isWork = true;
            isOff = true;
        }
        else if (collider == null && isOff)
        {
            onSprite.SetActive(true);
            offSprite.SetActive(false);
            if (audioManager)
                audioManager.Play("switch");
            isOff = false;
        }
    }
}
