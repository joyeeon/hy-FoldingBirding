using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDistanceChecker : MonoBehaviour
{
    public float maxDistance = 1.0f;
    private Transform hmdTransform;

    void Start()
    {
        hmdTransform = Camera.main.transform;
    }

    public bool IsCloseEnough()
    {
        float distance = Vector3.Distance(hmdTransform.position, transform.position);
        return distance <= maxDistance;
    }
}
