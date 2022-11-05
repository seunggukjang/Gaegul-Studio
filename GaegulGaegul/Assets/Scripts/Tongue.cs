using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    public Transform targetTransform = null;
    private Vector3 direction = new Vector3();
    public Transform tongueSprite;
    private Vector3 tongueSize = new Vector3();
    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }
    public void TurnOffTongue()
    {
        targetTransform = null;
    }
    void Start()
    {
        //direction = targetTransform.position - transform.position;
        tongueSize = tongueSprite.transform.localScale;
        //tongueSize.x = direction.magnitude;
        tongueSprite.transform.localScale = tongueSize;
    }

    void Update()
    {
        if(targetTransform == null)
        return;
        direction = targetTransform.position - transform.position;
        tongueSize.x = direction.magnitude;
        tongueSprite.transform.localScale = tongueSize;

        transform.rotation = Quaternion.FromToRotation(Vector3.right,direction);
        
    }
}
