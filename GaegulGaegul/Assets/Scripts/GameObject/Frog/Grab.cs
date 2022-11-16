using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grab : MonoBehaviour
{
    public float grabRangeRadius = 3f;
    public float mouseGrabThicknessTongue = 0.5f;
    [SerializeField] private Transform mouseGrabRange;
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
    private MousePosition mousePos;
    
    private Enemy holdingEnemy = null;
    void Start()
    {
        joint = GetComponent<SpringJoint2D>();
        mousePos = GetComponent<MousePosition>();
        //tongueFrog = tongueToPlayer.GetComponent<TongueFrog>();
        layers[0] = 1 << LayerMask.NameToLayer("Hook");
        //layers[1] = 1 << LayerMask.NameToLayer("Frog");
        layers[1] = 1 << LayerMask.NameToLayer("Item");
        layers[2] = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Item");
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
    private Vector3 GetFrogToMouseDirection()
    {
        return (mousePos.GetMousePosition() - transform.position).normalized;
    }
    private Rigidbody2D SelectTarget()
    {
        foreach(LayerMask l in layers)
        {
            RaycastHit2D[] targetHit2Ds = Physics2D.CircleCastAll(transform.position, mouseGrabThicknessTongue, GetFrogToMouseDirection(), mouseGrabRange.lossyScale.x * 0.5f, l);
            foreach(RaycastHit2D hit in targetHit2Ds)
            {
                if(hit)
                {
                    if(hit.transform.name == gameObject.name)
                        continue;
                    else
                    {
                        targetRB = hit.rigidbody;
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
            if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item")
            {
                if(targetTag == "Enemy" && !joint.enabled)
                {
                    holdingEnemy = targetRB.GetComponent<Enemy>();
                    holdingEnemy.gameObject.SetActive(true);
                    holdingEnemy.LaunchWithEnemy(GetFrogToMouseDirection(), fireForce, transform.position);
                    isGrab = false;
                    targetRB = null;
                    return false;
                }
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
            if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item")
            {
                joint.connectedBody = null;
                joint.enabled = false;
                tongueToObject.SetActive(false);
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
        if(!CanGrip() || SelectTarget() == null)
            return false;
        string targetTag = targetRB.gameObject.tag;
        if(targetTag == "Hook" || targetTag == "Enemy" || targetTag == "Item")
        {
            if(targetTag == "Hook")
            {
                joint.autoConfigureDistance = true;
                joint.frequency = 0;
                animator.SetTrigger("grab");
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
        //else if(targetTag == "Frog")
        //{
        //    tongueToPlayer.SetActive(true);
        //    animator.SetTrigger("grab");
        //    tongueFrog.SetJointTargetRigidBody(targetRB);
        //}
        isGrab = true;
        return true;
    }
}
