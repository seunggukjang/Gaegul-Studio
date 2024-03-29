using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSaw : MonoBehaviour
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform stopTransform;
    [SerializeField] private float smoothTime = 0.99f;
    [SerializeField] private float rotateVelocity = 500f;
    private float originRotateVelocity;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 stopPosition;
    private Vector3 startPosition;
    private bool isMove = false;
    private Vector3 halfSize;
    private LayerMask frogMask;
    private AudioManager audiomanager;
    private bool isSoundPlay = false;
    void Start()
    {
        originRotateVelocity = rotateVelocity;
        stopPosition = stopTransform.position;
        startPosition = startTransform.position;
        halfSize = transform.lossyScale * 0.4f;
        audiomanager = AudioManager.instance;
        audiomanager = FindObjectOfType<AudioManager>();
        frogMask = 1 << LayerMask.NameToLayer("Frog");
            
    }
    void Update()
    {
        if(Camera.main != null && audiomanager)
        {
            float cameradistance = (Camera.main.transform.position - transform.position).sqrMagnitude;
            if (cameradistance < 5)
            {
                if(isSoundPlay == false)
                {
                    audiomanager.Play("saw");
                    isSoundPlay = true;
                }
                if (cameradistance > 1)
                    audiomanager.SetVolume(1 / cameradistance);

            }
            else
            {
                audiomanager.Stop("saw");
                isSoundPlay=false;
            }
        }
        transform.Rotate(0,0, Time.deltaTime * rotateVelocity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, halfSize.x, frogMask);
        if(colliders.Length > 0)
        {
            rotateVelocity = originRotateVelocity * 2;
        }
        else
        {
            rotateVelocity = originRotateVelocity;
            if (isMove)
            {
                transform.position = Vector3.SmoothDamp(transform.position, stopPosition, ref velocity, smoothTime);
                if ((transform.position - stopPosition).sqrMagnitude < 0.1)
                {
                    isMove = false;
                }
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothTime);
                if ((transform.position - startPosition).sqrMagnitude < 0.1 )
                {
                    isMove = true;
                }
            }
        }
    }
}
