using UnityEngine;
using System.Collections;

public class HandModelsManager : MonoBehaviour
{
    public GameObject motionUI;
    public bool hideOthersOnShow = true;

    private static HandModelsManager[] allDisplayers;

    private void Awake()
    {
        if (allDisplayers == null)
        {
            allDisplayers = FindObjectsOfType<HandModelsManager>(true);
        }

        if (motionUI != null)
        {
            motionUI.SetActive(false);
        }
    }

    public void ShowMotion()
    {
        if (hideOthersOnShow)
        {
            foreach (var displayer in allDisplayers)
            {
                if (displayer != this && displayer.motionUI != null)
                {
                    displayer.motionUI.SetActive(false);
                }
            }
        }

        if (motionUI != null)
        {
            // 기존 방식: 바로 켜기
            // motionUI.SetActive(true);

            // 변경: 깜빡임 효과 실행
            StartCoroutine(BlinkMotion());
        }
    }

    private IEnumerator BlinkMotion()
    {
        motionUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        motionUI.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        motionUI.SetActive(true);
    }

    public void HideMotion()
    {
        if (motionUI != null)
        {
            motionUI.SetActive(false);
        }
    }
}
