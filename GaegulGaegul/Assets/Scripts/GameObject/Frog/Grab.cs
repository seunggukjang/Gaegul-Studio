using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grab : MonoBehaviour
{
    float grabRangeRadius = 5f;
    public float mouseGrabThicknessTongue = 5f;
    [SerializeField] private Transform grabRange;
    private SpringJoint2D joint;
    Rigidbody2D targetRB = null;
    Collider2D targetCollider;
    [SerializeField] private GameObject tongueToPlayer;
    private TongueFrog tongueFrog;
    [SerializeField] private GameObject tongueToObject;
    [SerializeField] private Tongue tongue;
    private LayerMask[] layers = new LayerMask[3];
    [SerializeField] private float fireForce = 400f;
    [SerializeField] private Animator animator;

    private bool isGrab = false;
    Vector3 mousePos;
    private Enemy holdingEnemy = null;
    void Start()
    {
        joint = GetComponent<SpringJoint2D>();
        mousePos = Input.mousePosition;
        //tongueFrog = tongueToPlayer.GetComponent<TongueFrog>();
        layers[0] = 1 << LayerMask.NameToLayer("Hook");
        //layers[1] = 1 << LayerMask.NameToLayer("Frog");
        layers[1] = 1 << LayerMask.NameToLayer("Item");
        layers[2] = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Weight");
        if (grabRange)
            grabRangeRadius = grabRange.lossyScale.x;
    }
    void Update()
    {
        if(!targetRB)
            return;
        if(isGrab && !targetRB.gameObject.activeSelf)
            CancelPulling();
    }
    public int GetTargeID()
    {
        if(!targetRB)
            return -1;
        return targetRB.gameObject.GetInstanceID();
    }
    
    private Rigidbody2D SelectTarget()
    {
        foreach(LayerMask l in layers)
        {
            Collider2D[] targetHit2Ds = Physics2D.OverlapCircleAll(transform.position, grabRangeRadius, l);
            foreach(Collider2D hit in targetHit2Ds)
            {
                if(hit)
                {
                    if(hit.transform.name == gameObject.name)
                        continue;
                        
                    else
                    {
                        targetRB = hit.transform.GetComponent<Rigidbody2D>();
                        return targetRB;
                    }
                }
            }
        }
        targetRB = null;
        return targetRB;
    }
    private bool CanGrip()
    {
        if(isGrab == true)
        {
            string targetTag = targetRB.gameObject.tag;
            if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item" || targetTag == "Weight")
            {
                if (targetTag == "Enemy" && !joint.enabled)
                {
                    holdingEnemy = targetRB.GetComponent<Enemy>();
                    holdingEnemy.gameObject.SetActive(true);
                    holdingEnemy.LaunchWithEnemy(transform.forward, fireForce, transform.position);
                    isGrab = false;
                    targetRB = null;
                    return false;
                }
                else if (targetTag == "Weight")
                    targetRB.GetComponent<Weight>().DecreaseGrabNumber();
                joint.connectedBody = null;
                joint.enabled = false;
                tongueToObject.SetActive(false);
                animator.SetTrigger("idle");
            }
            //else if(targetTag == "Frog")
            //{
            //    tongueFrog.SetJointTargetRigidBody(null);
            //    tongueToPlayer.SetActive(false);
            //    animator.SetTrigger("idle");
            //}
            isGrab = false;
            targetRB = null;
            return false;
        }

        return true;
    }
    public void CancelPulling()
    {
        if(isGrab == true)
        {
            string targetTag = targetRB.gameObject.tag;
            if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item" || targetTag == "Weight")
            {
                joint.connectedBody = null;
                joint.enabled = false;
                tongueToObject.SetActive(false);
                if(targetTag == "Weight")
                {
                    targetRB.GetComponent<Weight>().DecreaseGrabNumber();
                }
            }
            //else if(targetTag == "Frog")
            //{
            //    tongueFrog.SetJointTargetRigidBody(null);
            //    tongueToPlayer.SetActive(false);
            //    animator.SetTrigger("idle");
            //}
            if(targetTag != "Enemy")
            {
                isGrab = false;
                targetRB = null;
            }
        }
    }
    public void CancelAllPulling()
    {
        joint.connectedBody = null;
        joint.enabled = false;
        tongueToObject.SetActive(false);
        //tongueFrog.SetJointTargetRigidBody(null);
        tongueToPlayer.SetActive(false);
        isGrab = false;
        targetRB = null;
    }
    public bool Grip()
    {

        if(!CanGrip() || SelectTarget() == null) {
            return false;
        }

        string targetTag = targetRB.gameObject.tag;
        if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item" || targetTag == "Weight")
        {
            if(targetTag == "Item")
            {
                animator.SetTrigger("eat");
            }
            if(targetTag == "Hook" || targetTag == "Weight")
            {
                joint.autoConfigureDistance = true;
                joint.frequency = 0;
                animator.SetTrigger("grab");
                if (targetTag == "Weight")
                {
                    targetRB.gameObject.GetComponent<Weight>().IncreaseGrabNumber();
                }
            }
            else
            {
                joint.autoConfigureDistance = false;
                joint.distance = 1f;
                joint.frequency = 1;
                if(targetTag == "Enemy")
                {
                    targetRB.gameObject.layer = LayerMask.NameToLayer("Item");
                    holdingEnemy = targetRB.GetComponent<Enemy>();
                    holdingEnemy.SetFrogID(gameObject.GetInstanceID());
                    animator.SetTrigger("eat");
                }
            }
            joint.enabled = true;
            joint.connectedBody = targetRB;
            
            tongueToObject.SetActive(true);
            tongue.targetTransform = targetRB.transform;
        }
        isGrab = true;
        return true;
    }
}
