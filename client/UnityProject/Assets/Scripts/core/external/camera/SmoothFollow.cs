using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 20.0f;
    public float distanceSmoothTime = 0.0f;
    public float distanceAccelerate = 0.0f;
    public bool canDistanceAccelerate = false;

    public float height = 5.0f;
    public float heightDamping = 2.0f;

    public float rotationDamping = 0.3f;

    Rigidbody targetRigidbody;

    public void Awake()
    {

        if (target == null)
        {
            Debug.LogError(" not find target");
            return;
        }

        if (canDistanceAccelerate)
        {
            targetRigidbody = target.GetComponent<Rigidbody>();
        }
        else
        {
            targetRigidbody = null;
        }
    }

    float usedDistance;
    float zVelocity = 0.0F;
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        var wantedRotationAngle = target.eulerAngles.y;
        var currentRotationAngle = transform.eulerAngles.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        var wantedHeight = target.position.y + height;
        var currentHeight = transform.position.y;
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var wantedPosition = target.position;
        wantedPosition.y = currentHeight;

        var usedDistanceAccelerate = 0.0f;
        if(canDistanceAccelerate)
        {
            usedDistanceAccelerate = targetRigidbody.velocity.magnitude * distanceAccelerate;
        }

        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + usedDistanceAccelerate, ref zVelocity, distanceSmoothTime);

        var temp = Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, usedDistance);
        wantedPosition -= temp;

        transform.position = wantedPosition;

        transform.LookAt(target.position);
    }

}