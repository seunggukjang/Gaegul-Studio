using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform stopTransform;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Trigger[] pieces;
    private int piecesCount = 0;
    private Vector3 velocity = Vector3.zero;
    private Vector3 stopPosition;
    private bool isTriggerOver = false;
    private bool isMove = false;
    void Start()
    {
        stopPosition = stopTransform.position;
        piecesCount = pieces.Length;
    }
    void Update()
    {
        if(isTriggerOver)
            return;
        if(isMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, stopPosition, ref velocity, smoothTime);
            if(transform.position == stopPosition)
            {
                isMove = false;
                isTriggerOver = true;
            }
        }
        int triggeredPieces = 0;
        for(int i = 0; i < piecesCount; i++)
        {
            if(pieces[i].GetIsWork())
            {
                triggeredPieces++;
            }
            if(triggeredPieces >= piecesCount)
                isMove = true;
        }
    }
}
