using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform range;
    private LayerMask frogLayer;
    private int holdingFrogID = -1;
    private bool isInFrog = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frogLayer = 1 << LayerMask.NameToLayer("Frog");
    }
    void FixedUpdate()
    {
        Collider2D frogHit2D = Physics2D.OverlapCircle(transform.position, range.lossyScale.x * 0.5f + 0.2f, frogLayer);
        if(frogHit2D != null && frogHit2D.gameObject.GetInstanceID() == holdingFrogID)
        {
            
            Grab grab = frogHit2D.transform.GetComponent<Grab>();
            
            if(grab.GetTargeID() == gameObject.GetInstanceID())
            {
                grab.CancelPulling();
                gameObject.SetActive(false);
                isInFrog = true;
                holdingFrogID = -1;
            }
        }
    }
    public void SetFrogID(int id)
    {
        holdingFrogID = id;
    }
    
    public void LaunchWithEnemy(Vector3 direction, float force, Vector3 frogPosition)
    {
        isInFrog = false;
        transform.position = frogPosition + (direction * rb.transform.lossyScale.magnitude);
        rb.AddForce(direction * force);
    }
    public bool IsInFrog()
    {
        return isInFrog;
    }
}
