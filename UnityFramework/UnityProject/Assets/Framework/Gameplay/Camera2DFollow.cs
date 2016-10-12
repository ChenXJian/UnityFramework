using System;
using UnityEngine;


public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;

    float distanceZ;
    Vector3 targetPosLast;

    void Start()
    {
        targetPosLast = target.position;
        distanceZ = (transform.position - target.position).z;
    }

    Vector3 vVelocity;
    void LateUpdate()
    {
        float xMoveDelta = (target.position - targetPosLast).x;

        var lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);

        Vector3 aheadTargetPos = target.position + (Vector3.forward * distanceZ) + lookAheadPos;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref vVelocity, damping);

        transform.position = newPos;

        targetPosLast = target.position;
    }
}
