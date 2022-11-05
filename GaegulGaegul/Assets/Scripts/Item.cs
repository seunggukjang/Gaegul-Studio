using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Transform range;
    [SerializeField] private LayerMask frogLayer;
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
            gameObject.SetActive(false);
        }
    }
}
