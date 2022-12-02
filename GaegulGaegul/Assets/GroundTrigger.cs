using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : Trigger
{
    
    Vector2 halfSize;
    Vector2 position;
    LayerMask frogMask;
    // Start is called before the first frame update
    void Start()
    {
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
        if(collider)
        {
            Debug.Log("Work");
            this.isWork = true;
        }
    }
}
