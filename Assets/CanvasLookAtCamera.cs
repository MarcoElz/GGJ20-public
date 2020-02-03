﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);

        Vector3 euler = transform.localRotation.eulerAngles;
        euler.y = 180f;
        transform.localRotation = Quaternion.Euler(euler);
    }
}
