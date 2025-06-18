using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDistanceManager : MonoBehaviour
{
    public string birdTag = "MyBird";
    public float maxDistance = 0.5f;

    public Transform hmdTransform;

    void Start()
    {
        if (hmdTransform == null)
        {
            if (Camera.main != null)
            {
                hmdTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("[BirdDistanceManager] Camera.main�� null�Դϴ�. hmdTransform�� �������� �Ҵ��ؾ� �մϴ�.");
            }
        }
    }

    // �ܺο��� ȣ���ؼ� ���� Ȯ��
    public bool IsAnyBirdClose()
    {
        if (hmdTransform == null)
        {
            Debug.LogError("[BirdDistanceManager] hmdTransform�� null�̶� �Ÿ� ��� �Ұ�!");
            return false;
        }

        GameObject[] birds = GameObject.FindGameObjectsWithTag(birdTag);

        foreach (GameObject bird in birds)
        {
            if (bird == null) continue;

            float distance = Vector3.Distance(hmdTransform.position, bird.transform.position);
            Debug.Log($"[BirdDistanceManager] HMD �� {bird.name} �Ÿ�: {distance:F2}m");

            if (distance <= maxDistance)
            {
                Debug.Log("[BirdDistanceManager] ����� �� �߰�: ���ͷ��� ���");
                if (StateManager.instance != null)
                {
                    StateManager.instance.birdState = StateManager.BirdState.Follow;
                }
                else
                {
                    Debug.LogWarning("[BirdDistanceManager] StateManager.instance�� null�Դϴ�.");
                }
                return true;
            }
        }

        Debug.Log("[BirdDistanceManager] ��� ���� �ʹ� �ִ�: ���ͷ��� ����");
        return false;
    }


}
