using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public static MinimapCamera Instance;
    public GameObject Target;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 targetPosition = Target.transform.position;
        targetPosition.y = 10f;
        transform.position = targetPosition;

        Vector3 targetRotation = Target.transform.eulerAngles;
        targetRotation.x = 90;
        transform.eulerAngles = targetRotation;
    }
}
