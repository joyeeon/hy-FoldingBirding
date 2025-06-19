using Oculus.Interaction.Input;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
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

    //���� �� ȸ����
    private Quaternion defaultRotation;

    void Start()
    {
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
                Vector3 offset = new Vector3(-0.14f, -0.016f, 0.08f);
                Vector3 finalPosition = pose.position + pose.rotation * offset;
                bird.transform.position = finalPosition;
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


        //if (isSelected)
        //{
        //    handRef.Hand.GetJointPose(HandJointId.HandIndex2, out pose);
        //    bird.transform.position = pose.position;
        //}
    }



    public void OnFingerEvent (bool isFingerUp)
    {
        //Debug.Log("OnFingerEvent ȣ���: " + isFingerUp);
        isSelected = isFingerUp;
        //StateManager.instance.SetInteraction(InteractionState.Finger);
        if (!isFingerUp)
        {
            isBirdAttached = false; // �հ��� ������ �ٽ� ������
        }
    }

    public void OnLog(string msg)
    { 
        Debug.Log(msg); 
    }


}
