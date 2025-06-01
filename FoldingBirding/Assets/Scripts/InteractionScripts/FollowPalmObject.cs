using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//손바닥 뻗으면 위에 새가 앉는 스크립트 
public class FollowPalmObject : MonoBehaviour
{
    public Transform palmTransform;  // 손 Transform (예: RightHand)
    public Vector3 localOffset = new Vector3(0, -0.1f, 3f); // 손 기준 offset (로컬 좌표계)
    public float followSpeed = 10f;

    private bool shouldFollow = false;

    void Update()
    {
        if (!shouldFollow || palmTransform == null) return;

        // palmTransform 기준의 로컬 offset을 월드 좌표로 변환
        Vector3 targetPosition = palmTransform.TransformPoint(localOffset);

        // 위치 따라가기
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // 회전도 따라가기 (선택)
        //transform.rotation = Quaternion.Slerp(transform.rotation, palmTransform.rotation, Time.deltaTime * followSpeed);
    }

    public void StartFollowing() => shouldFollow = true;
    public void StopFollowing() => shouldFollow = false;
}
