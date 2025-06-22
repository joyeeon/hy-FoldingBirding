using UnityEngine;

public class HandModelsManager : MonoBehaviour
{
    [Tooltip("이 버튼이 선택되었을 때 보여줄 Motion UI 오브젝트")]
    public GameObject motionUI;

    [Tooltip("다른 MotionDisplayer들이 보여준 UI를 숨기려면 true")]
    public bool hideOthersOnShow = true;

    private static HandModelsManager[] allDisplayers;

    private void Awake()
    {
        // 처음 한 번만 전체 목록 확보
        if (allDisplayers == null)
        {
            allDisplayers = FindObjectsOfType<HandModelsManager>();
        }

        // 시작 시에는 꺼두기 (원하면 제거 가능)
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
            Debug.Log($"[MotionDisplayer] ShowMotion() 실행됨. motionUI: {motionUI?.name}");

            if (motionUI != null)
                motionUI.SetActive(true);
        }

        if (motionUI != null)
        {
            motionUI.SetActive(true);
        }
    }

    public void HideMotion()
    {
        if (motionUI != null)
        {
            motionUI.SetActive(false);
        }
    }
}
