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

    // 외부에서 호출해서 조건 확인
    public bool IsAnyBirdClose()
    {
        GameObject[] birds = GameObject.FindGameObjectsWithTag(birdTag);

        foreach (GameObject bird in birds)
        {
            float distance = Vector3.Distance(hmdTransform.position, bird.transform.position);
            Debug.Log($"[BirdDistanceManager] HMD ↔ {bird.name} 거리: {distance:F2}m");

            if (distance <= maxDistance)
            {
                Debug.Log("[BirdDistanceManager] 가까운 새 발견: 인터랙션 허용");
                return true;
            }
        }

        Debug.Log("[BirdDistanceManager] 모든 새가 너무 멀다: 인터랙션 차단");
        return false;
    }


}
