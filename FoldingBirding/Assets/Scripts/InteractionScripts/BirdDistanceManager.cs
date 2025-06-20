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
                Debug.LogError("[BirdDistanceManager] Camera.main이 null입니다. hmdTransform을 수동으로 할당해야 합니다.");
            }
        }
    }

    // 외부에서 호출해서 조건 확인
    public bool IsAnyBirdClose()
    {
        if (hmdTransform == null)
        {
            Debug.LogError("[BirdDistanceManager] hmdTransform이 null이라 거리 계산 불가!");
            return false;
        }

        GameObject[] birds = GameObject.FindGameObjectsWithTag(birdTag);

        foreach (GameObject bird in birds)
        {
            if (bird == null) continue;

            float distance = Vector3.Distance(hmdTransform.position, bird.transform.position);
            Debug.Log($"[BirdDistanceManager] HMD ↔ {bird.name} 거리: {distance:F2}m");

            if (distance <= maxDistance)
            {
                Debug.Log("[BirdDistanceManager] 가까운 새 발견: 인터랙션 허용");
                if (StateManager.instance != null)
                {
                    StateManager.instance.birdState = StateManager.BirdState.Follow;
                }
                else
                {
                    Debug.LogWarning("[BirdDistanceManager] StateManager.instance가 null입니다.");
                }
                return true;
            }
        }

        Debug.Log("[BirdDistanceManager] 모든 새가 너무 멀다: 인터랙션 차단");
        return false;
    }


}
