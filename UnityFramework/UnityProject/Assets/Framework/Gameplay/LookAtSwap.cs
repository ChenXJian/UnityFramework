using UnityEngine;
using System.Collections;

public class LookAtSwap : MonoBehaviour
{
    public Transform[] targets;
    public Transform targetCurrent;
    public float damping = 6;
    public int maxTargetIndex = 1;
    public int targetIndex;

    public bool isSmooth = true;
    public bool canSwap = true;

    void Start()
    {
        maxTargetIndex = targets.Length - 1;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    public void NextTarget()
    {
        if (targetIndex >= maxTargetIndex)
        {
            targetIndex = 0;
        }
        else
        {
            targetIndex++;
        }
    }

    public void LookAt(int index)
    {
        if (index > maxTargetIndex || index < 0)
        {
            targetIndex = 0;
        }
        else
        {
            targetIndex = index;
        }
    }

    void LateUpdate()
    {
        if (canSwap == true)
        {
            if (targetIndex > maxTargetIndex || targetIndex < 0)
            {
                Debug.Log("targetIndex error");
            }
            else
            {
                targetCurrent = targets[targetIndex];
            }
        }

        if (targetCurrent)
        {
            if (isSmooth)
            {
                var rotation = Quaternion.LookRotation(targetCurrent.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                transform.LookAt(targetCurrent);
            }
        }
    }
}