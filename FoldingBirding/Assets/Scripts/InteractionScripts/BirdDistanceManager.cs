using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDistanceManager : MonoBehaviour
{
    public string birdTag = "MyBird";
    public float maxDistance = 0.5f;

    private Transform hmdTransform;

    void Start()
    {
        hmdTransform = Camera.main.transform;
    }

    // �ܺο��� ȣ���ؼ� ���� Ȯ��
    public bool IsAnyBirdClose()
    {
        GameObject[] birds = GameObject.FindGameObjectsWithTag(birdTag);

        foreach (GameObject bird in birds)
        {
            float distance = Vector3.Distance(hmdTransform.position, bird.transform.position);
            Debug.Log($"[BirdDistanceManager] HMD �� {bird.name} �Ÿ�: {distance:F2}m");

            if (distance <= maxDistance)
            {
                Debug.Log("[BirdDistanceManager] ����� �� �߰�: ���ͷ��� ���");
                StateManager.instance.birdState = StateManager.BirdState.Follow;
                return true;
            }
        }

        Debug.Log("[BirdDistanceManager] ��� ���� �ʹ� �ִ�: ���ͷ��� ����");
        return false;
    }


}
