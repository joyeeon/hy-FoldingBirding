using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



//�չٴ� ������ ���� ���� �ɴ� ��ũ��Ʈ 
public class FollowPalmObject : MonoBehaviour
{
    public Transform palmTransform;
    public Vector3 localOffset = new Vector3(-1.5f, 3f, 0);
    public float followSpeed = 10f;

    private bool isPalmGestureActive = false;
    private BirdDistanceManager distanceManager;
    private BirdFollower birdFollower;

    //���� �� ȸ����
    private Quaternion defaultRotation;
    private Quaternion fixedRotation = Quaternion.Euler(0f, 180f, 0f);
    private bool isReturningRotation = false;

    void Start()
    {
        distanceManager = FindObjectOfType<BirdDistanceManager>();
        birdFollower = GetComponent<BirdFollower>(); // ���� ������Ʈ�� �پ��ִٰ� ����
        if (TryGetComponent(out Transform tf))
        {
            defaultRotation = tf.rotation; // �ʱ� ȸ���� ����
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
            // ȸ�� ������� �ǵ�����
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
            // �⺻ ������� ����
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
