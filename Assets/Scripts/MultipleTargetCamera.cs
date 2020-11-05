using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;
    public float smoothTime = .5f;

    private Vector3 velocity;
    private Camera cam;

    [Header("Parameter")]
    [SerializeField]
    float boxSizeMin;
    [SerializeField]
    float boxSizeMax;

    [SerializeField]
    Vector2 offsetZoomZ;

    Vector3 offsetZ;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        //
        float multiplierZ = 0;
        if (bounds.size.magnitude > boxSizeMin)
        {
            multiplierZ = (bounds.size.magnitude - boxSizeMin) / (boxSizeMax - boxSizeMin);
            multiplierZ = Mathf.Clamp(multiplierZ, 0, 1);
        }
        offsetZ = new Vector3(0, 0, multiplierZ * (offsetZoomZ.x - offsetZoomZ.y));


        return bounds.center - offsetZ;
    }
}
