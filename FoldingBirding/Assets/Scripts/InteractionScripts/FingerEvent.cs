using Oculus.Interaction.Input;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FingerEvent : MonoBehaviour
{
    private HandRef handRef;

    private Pose pose;

    private GameObject bird;

    public float followSpeed = 3f;
    public float attachDistance = 0.1f;
    public Vector3 localOffset = new Vector3(0f, 0.0f, 0f);
    public bool isSelected;

    public bool isBirdAttached = false;
    private BirdDistanceManager distanceManager;

    //���� �� ȸ����
    private Quaternion defaultRotation;

    void Start()
    {
        distanceManager = FindObjectOfType<BirdDistanceManager>();
        bird = GameObject.FindWithTag("MyBird"); //�� ������Ʈã��
        handRef = GetComponent<HandRef>(); //�� ã�� 
        if (handRef == null)
        {
            Debug.LogError("HandRef component not found on the GameObject.");
            return;
        }

        if (bird != null)
            defaultRotation = bird.transform.rotation;
        // Subscribe to the OnLog event
        //OVRManager.OnLog += OnLog;
    }

    private void Update()
    {
        if (!isSelected || bird == null || handRef == null)
        {
            // ȸ�� ���� �õ�
            if (!isBirdAttached && bird != null)
            {
                bird.transform.rotation = Quaternion.Slerp(
                    bird.transform.rotation,
                    defaultRotation,
                    Time.deltaTime * 5f
                );
            }
            return;
        }

        if (handRef.Hand.GetJointPose(HandJointId.HandIndex2, out pose))
        {
            Vector3 targetPos = pose.position + pose.rotation * localOffset;
            float distance = Vector3.Distance(bird.transform.position, targetPos);

            if (!isBirdAttached && distance <= attachDistance)
            {
                // ���� �հ��� ������ ���� �� ����
                isBirdAttached = true;
                Debug.Log("���� �հ����� ����!");
            }

            if (isBirdAttached)
            {
                // �հ��� ���� ���̱�
                Vector3 offset = new Vector3(-0.42f, -0.07f, 0.2f);
                //Vector3 finalPosition = pose.position + pose.rotation * offset;
                Vector3 targetPosition = pose.position + pose.rotation * offset;

                bird.transform.position = Vector3.Lerp(
                bird.transform.position,
                targetPosition,
                Time.deltaTime * 15f // 5~15 ������ ���� ����
    );
                bird.transform.rotation = pose.rotation * Quaternion.Euler(180f, 180f, -90f); // ���� ����
            }
            else
            {
                // �������(Lerp)
                bird.transform.position = Vector3.MoveTowards(
                    bird.transform.position,
                    targetPos,
                    Time.deltaTime * followSpeed
                );
            }
        }

    }




    public void OnFingerEvent(bool isFingerUp)
    {
        if (isFingerUp)
        {
            if (distanceManager != null && distanceManager.IsAnyBirdClose())
            {
                isSelected = true;
                isBirdAttached = false; // ���� �ʱ�ȭ
                Debug.Log("�� �հ��� ��ħ �� ����: Finger");
                StateManager.instance.SetInteractionState(StateManager.InteractionState.Finger);
            }
        }
        else
        {
            isSelected = false;
            isBirdAttached = false;
            Debug.Log("�� �հ��� ���� �� ����: Follow");
            StateManager.instance.SetInteractionState(StateManager.InteractionState.Follow);
        }
    }

    public void OnLog(string msg)
    { 
        Debug.Log(msg); 
    }


}
