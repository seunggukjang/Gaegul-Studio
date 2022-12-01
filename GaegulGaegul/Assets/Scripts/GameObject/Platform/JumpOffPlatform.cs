using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOffPlatform : MonoBehaviour
{
    private LayerMask frogMask;
    Vector2 position;
    Vector2 halfSize;
    private void Start()
    {
        frogMask = 1 << LayerMask.NameToLayer("Frog");
        position = transform.position;
        halfSize = transform.lossyScale;

    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(position - halfSize, position + halfSize, frogMask);
        foreach(Collider2D collider in colliders)
        {

        }
    }
}
