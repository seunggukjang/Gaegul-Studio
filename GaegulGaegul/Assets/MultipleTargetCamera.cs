using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    public Vector3 offset;

    public float smoothTime = .5f;
    public float minZoom = 60f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    public float minZoomOrthogonal = 4f;
    public float maxZoomOrthogonal = 20f;
    public float zoomLimiterOrthogonal = 50f;
    private Vector3 velocity;
    private Camera cam;


    
    // public Transform targetTransform;
    // private Vector3 position = new Vector3();
    
    void Start()
    {
        Player[] frogs = FindObjectsOfType(typeof(Player), false) as Player[];
        for (int i = 0; i < frogs.Length; i++)
        {
            targets.Add(frogs[i].transform);
        }
        cam = GetComponent<Camera>();
    }
    // void Update()
    // {
    //     position = targetTransform.position;
    //     position.z = -10;
    //     transform.position = position;
    // }
    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom;
        if (cam.orthographic == false)
        {
             newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }
        else
        {
             newZoom = Mathf.Lerp(minZoomOrthogonal, maxZoomOrthogonal, GetGreatestDistance() / zoomLimiterOrthogonal);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime * zoomLimiterOrthogonal);
        }
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
