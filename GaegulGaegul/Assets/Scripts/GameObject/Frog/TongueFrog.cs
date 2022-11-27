using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueFrog : MonoBehaviour
{
    [SerializeField] private GameObject lastTongue;
    private SpringJoint2D joint;

    void Awake()
    {
        joint = lastTongue.GetComponent<SpringJoint2D>();
    }

    public void SetJointTargetRigidBody(Rigidbody2D rd)
    {
        if(!joint)
            return;
        joint.connectedBody = rd;
    }
}
