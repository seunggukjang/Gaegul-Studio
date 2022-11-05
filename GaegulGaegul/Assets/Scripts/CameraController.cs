using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform targetTransform;

    private Vector3 position = new Vector3();
    void Start()
    {
        position = targetTransform.position;
        position.z = -10;
    }

    // Update is called once per frame
    void Update()
    {
        position = targetTransform.position;
        position.z = -10;
        transform.position = position;
    }
}
