using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAboveHeadDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform headTransform;  // HMD, 즉 머리 위치
    public Transform handTransform;  // 손 위치 (왼손 or 오른손)
    public float yOffset = 0.15f;    // 머리보다 얼마나 위로 올라가야 하는지 기준

    private bool hasTriggered = false;

    void Update()
    {
        float headY = headTransform.position.y;
        float handY = handTransform.position.y;

        // 매 프레임 손과 머리의 Y 좌표 로그 출력
        Debug.Log($"[HandAboveHeadDetector] Head Y: {headY:F2}, Hand Y: {handY:F2}");

        if (handY > headY + yOffset)
        {
            if (!hasTriggered)
            {
                Debug.Log("[HandAboveHeadDetector] 손이 머리 위로 올라갔습니다!");
                hasTriggered = true;
            }
        }
        else
        {
            hasTriggered = false;
        }
    }
}
