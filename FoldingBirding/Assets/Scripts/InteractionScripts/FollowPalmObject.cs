using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



//손바닥 뻗으면 위에 새가 앉는 스크립트 
public class FollowPalmObject : MonoBehaviour
{
    //private BirdDistanceChecker birdDistance;
    //public Transform palmTransform;  // 손 Transform (예: RightHand)
    //public Vector3 localOffset = new Vector3(0, -0.1f, 3f); // 손 기준 offset (로컬 좌표계)
    //public float followSpeed = 10f;

    ////hmd와 새 사이의 거리 계산 
    //public float maxAllowedDistance = 0.5f;

    //private Transform hmdTransform;
    //private bool isPalmGestureActive = false;
    ////private bool shouldFollow = false;

    //void Start()
    //{
    //    // HMD 위치 설정(메인 카메라 위치)
    //    hmdTransform = Camera.main.transform;
    //    birdDistance = GetComponent<BirdDistanceChecker>();
    //}

    //void Update()
    //{
    //    //if (!shouldFollow || palmTransform == null) return;

    //    if (palmTransform == null || hmdTransform == null) return;

    //    //hmd와 새 사이의 거리 계산. 
    //    float distance = Vector3.Distance(hmdTransform.position, palmTransform.position);
    //    if (!isPalmGestureActive || distance > maxAllowedDistance)
    //        return;


    //    //if (!shouldFollow) return;

    //    // palmTransform 기준의 로컬 offset을 월드 좌표로 변환
    //    Vector3 targetPosition = palmTransform.TransformPoint(localOffset);

    //    // 위치 따라가기
    //    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

    //    Debug.Log($"Gesture: {isPalmGestureActive}, Distance: {distance}");
    //    // 회전도 따라가기 (선택)
    //    //transform.rotation = Quaternion.Slerp(transform.rotation, palmTransform.rotation, Time.deltaTime * followSpeed);
    //}

    ////public void StartFollowing() => shouldFollow = true;
    ////public void StopFollowing() => shouldFollow = false;

    //public void OnPalmGestureDetected()
    //{
    //    if (birdDistance != null && birdDistance.IsCloseEnough())
    //    {
    //        isPalmGestureActive = true;
    //    }
    //}

    //public void OnPalmGestureEnded()
    //{
    //    isPalmGestureActive = false;
    //}
    public Transform palmTransform;
    public Vector3 localOffset = new Vector3(0, -0.1f, 3f);
    public float followSpeed = 10f;

    private bool isPalmGestureActive = false;
    private BirdDistanceManager distanceManager;

    void Start()
    {
        distanceManager = FindObjectOfType<BirdDistanceManager>();
    }

    void Update()
    {
        if (!isPalmGestureActive || palmTransform == null) return;

        Vector3 targetPosition = palmTransform.TransformPoint(localOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    public void OnPalmGestureDetected()
    {
        if (distanceManager != null && distanceManager.IsAnyBirdClose())
        {
            isPalmGestureActive = true;
        }
    }

    public void OnPalmGestureEnded()
    {
        isPalmGestureActive = false;
    }


}
