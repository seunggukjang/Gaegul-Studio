using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    private Vector3 halfSize;
    private LayerMask frogLayer;
    private bool isPlay = false;
    void Start()
    {
        frogLayer = 1 << LayerMask.NameToLayer("Frog");
        halfSize = transform.lossyScale * 0.5f;
        isPlay = false;
    }

        private void FixedUpdate()
    {
        
        Collider2D frogCollider = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, frogLayer);
        if(frogCollider && !isPlay)
        {
            particles.Play();
            isPlay = true;
        }
    }
}
