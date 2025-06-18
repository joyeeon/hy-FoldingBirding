using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollower : MonoBehaviour
{
    public Transform hmdTransform;              // HMD ��ġ
    public float followDistance = 0.7f;         // ����� �Ÿ� (��: 1m)
    public float followHeightOffset = 0.3f;     // �ణ ���� �ֵ���
    public float followSpeed = 3.0f;

    public float maxFollowStartDistance = 0.7f;  // �� �Ÿ� �̳��� ������� ����
    public float stopFollowDistance = 1.5f;      // �� �Ÿ� ������ ������� ����


    public bool isFollowing = true;             // ���ο��� ����
    private bool externallyDisabled = false; //�ܺο��� ���� ����


    // Update is called once per frame
    void Update()
    {
        if (hmdTransform == null || externallyDisabled)
            return;

        float currentDistance = Vector3.Distance(transform.position, hmdTransform.position);

        // ���� �ʹ� �־������� ����
        if (isFollowing && currentDistance > stopFollowDistance)
        {
            Debug.Log("[BirdFollower] ���� �ʹ� �־������ϴ�. ������� ����.");
            isFollowing = false;
        }

        // ���� ����������� ������� ����
        if (!isFollowing && currentDistance <= maxFollowStartDistance)
        {
            Debug.Log("[BirdFollower] ���� ����������ϴ�. ������� �����մϴ�.");
            isFollowing = true;
        }

        // ������� ����
        if (isFollowing)
        {
            //Vector3 targetPosition = hmdTransform.position +
            //                         hmdTransform.forward * followDistance +
            //                         Vector3.up * followHeightOffset;

            Vector3 localOffset = new Vector3(0.5f, -0.3f, 0.7f);  // ������ + ��
            Vector3 targetPosition = hmdTransform.TransformPoint(localOffset);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }

    public void SetFollowing(bool shouldFollow)
    {
        isFollowing = shouldFollow;
    }

    // �ܺο��� ȣ��: �ڵ� ���� ����/����
    public void SetExternalControl(bool disabled)
    {
        Debug.Log($"[BirdFollower] �ܺ� ���� ����: {(disabled ? "��Ȱ��ȭ" : "Ȱ��ȭ")}");
        externallyDisabled = disabled;
    }

}
