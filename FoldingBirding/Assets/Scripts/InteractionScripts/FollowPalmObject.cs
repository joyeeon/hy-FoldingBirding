using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



//손바닥 뻗으면 위에 새가 앉는 스크립트 
public class FollowPalmObject : MonoBehaviour
{
    public Transform palmTransform;
    public Vector3 localOffset = new Vector3(-1.5f, 3f, 0);
    public float followSpeed = 10f;

    private bool isPalmGestureActive = false;
    private BirdDistanceManager distanceManager;
    private BirdFollower birdFollower;

    //원래 새 회전값
    private Quaternion defaultRotation;
    private Quaternion fixedRotation = Quaternion.Euler(0f, 180f, 0f);
    private bool isReturningRotation = false;

    void Start()
    {
        distanceManager = FindObjectOfType<BirdDistanceManager>();
        birdFollower = GetComponent<BirdFollower>(); // 같은 오브젝트에 붙어있다고 가정
        if (TryGetComponent(out Transform tf))
        {
            defaultRotation = tf.rotation; // 초기 회전값 저장
        }

    }

    void Update()
    {
        if (!palmTransform) return;

        if (isPalmGestureActive)
        {
            Vector3 targetPosition = palmTransform.TransformPoint(localOffset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

            
            transform.rotation = Quaternion.Slerp(transform.rotation, fixedRotation, Time.deltaTime * 10f);
            isReturningRotation = false;
        }
        else if (isReturningRotation)
        {
            // 회전 원래대로 되돌리기
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * 5f);
        }

        //Vector3 targetPosition = palmTransform.TransformPoint(localOffset);
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * followSpeed);


    }

    public void OnPalmGestureDetected()
    {
        if (distanceManager != null && distanceManager.IsAnyBirdClose())
        {
            isPalmGestureActive = true;
            // 기본 따라오기 멈춤
            birdFollower?.SetExternalControl(true);
            StateManager.instance.SetInteractionState(StateManager.InteractionState.Palm);
        }
    }

    public void OnPalmGestureEnded()
    {
        isPalmGestureActive = false;
        birdFollower?.SetExternalControl(false);
        //GetComponent<BirdFollower>()?.SetFollowing(true);
        StateManager.instance.SetInteractionState(StateManager.InteractionState.Follow);
    }


}
