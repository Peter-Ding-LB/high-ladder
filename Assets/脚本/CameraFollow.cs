using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.08f;
    public Vector3 offset;
    private bool initDone;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        if (!initDone)
        {
            transform.position = targetPos;
            initDone = true;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
    }
}
