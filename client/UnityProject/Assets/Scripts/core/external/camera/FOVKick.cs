using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class FOVKick
{
    public Camera Camera;
    public float FOVIncrease = 3f;
    public AnimationCurve IncreaseCurve;

    float originalFov;

    public void Setup(Camera camera)
    {
        CheckStatus(camera);

        Camera = camera;
        originalFov = camera.fieldOfView;
    }

    private void CheckStatus(Camera camera)
    {
        if (camera == null)
        {
            Debug.LogError("FOVKick camera is null");
        }

        if (IncreaseCurve == null)
        {
            Debug.LogError("FOVKick Increase curve is null");
        }
    }

    public void ChangeCamera(Camera camera)
    {
        Camera = camera;
    }

    public IEnumerator FOVKickUp()
    {
        float t = Mathf.Abs((Camera.fieldOfView - originalFov) / FOVIncrease);

        while (t < 1f)
        {
            Camera.fieldOfView = originalFov + (IncreaseCurve.Evaluate(t) * FOVIncrease);
            Debug.Log(IncreaseCurve.Evaluate(t) * FOVIncrease);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FOVKickDown()
    {
        float t = Mathf.Abs((Camera.fieldOfView - originalFov) / FOVIncrease);
        while (t > 0)
        {
            Camera.fieldOfView = originalFov + (IncreaseCurve.Evaluate(t) * FOVIncrease);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Camera.fieldOfView = originalFov;
    }
}