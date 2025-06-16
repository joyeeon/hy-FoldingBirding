using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAboveHeadDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform headTransform;  // HMD, �� �Ӹ� ��ġ
    public Transform handTransform;  // �� ��ġ (�޼� or ������)
    public float yOffset = 0.15f;    // �Ӹ����� �󸶳� ���� �ö󰡾� �ϴ��� ����

    private GameObject bird;
    private bool isMovingBird = false;

    public float birdMoveSpeed = 2f;


    void Start()
    {
        bird = GameObject.FindWithTag("MyBird");
    }


    void Update()
    {
        float headY = headTransform.position.y;
        float handY = handTransform.position.y;
        
        // �� ������ �հ� �Ӹ��� Y ��ǥ �α� ���
        //Debug.Log($"[HandAboveHeadDetector] Head Y: {headY:F2}, Hand Y: {handY:F2}");

        if (handY > headY + yOffset)
        {
            if (!isMovingBird)
            {
                Debug.Log("[HandAboveHeadDetector] ���� �Ӹ� ���� �ö󰬽��ϴ�!");
                isMovingBird = true;
            }
        }
        else
        {
            isMovingBird = false;
            //bird.GetComponent<BirdFollower>()?.SetExternalControl(false); // �ٽ� �������
        }

        if (isMovingBird && bird != null)
        {
            //������� �� ����
            //bird.GetComponent<BirdFollower>()?.SetExternalControl(true);
            Vector3 target = headTransform.position + headTransform.forward * 0.5f;

            bird.transform.position = Vector3.Lerp(bird.transform.position, headTransform.position + headTransform.forward * 0.5f, Time.deltaTime * birdMoveSpeed);
        }
    }
}
