using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static StateManager;

public class TutorialManager : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject tutorialgCanvas;
    [SerializeField] private TMP_Text DescriptionTxt;

    [Header("Debug")]
    [SerializeField] private bool isWaitingInteraction = false;

    [Header("Step별 표시할 손 오브젝트")]
    [SerializeField] private List<GameObject> objectsToToggle;


    private List<(StateManager.InteractionState state, string desc, string result)> dialogueSteps = new()
    {
        (StateManager.InteractionState.Call,   "손을 뻗어 새를 불러 보세요!", "당신의 부름에 새가 반응했어요!"),
        (StateManager.InteractionState.Finger, "손가락 하나만 쭉! 새가 앉을 자리를 만들어주세요!", "새가 당신의 손가락 위에 앉았어요!"),
        (StateManager.InteractionState.Palm,   "손바닥을 펼쳐보세요.", "당신의 손이 새에게는 가장 편안한 자리예요."),
        (StateManager.InteractionState.Pet,    "새의 머리를 조심스레 쓰다듬어보세요.", "새가 기뻐하고 있어요! 당신과 새 사이에 신뢰가 생겼어요."),
        (StateManager.InteractionState.Follow, "함께 걸어볼까요? 멈추면 반응해요!", "함께 걷고 있어요. 당신의 곁에서 걷는 걸 새도 좋아하는 것 같아요."),
        (StateManager.InteractionState.Bye,    "다시 만나는 날을 기약하며 새를 떠나 보내요.", "새가 인사를 남기고 멀어졌어요. 오늘의 만남, 기억해 주세요!"),
    };

    private void Start()
    {
        StartCoroutine(InitTutorial());
    }

    private IEnumerator InitTutorial()
    {
        yield return StartCoroutine(SetCanvas());
        StartTutorial();
    }

    public void StartTutorial()
    {
        StartInteraction(0);
    }

    private IEnumerator SetCanvas()
    {
        yield return new WaitForSeconds(0.3f);

        Debug.Log(">>>>>>>>>>>>>>>> Start Setting Canvas");
        Camera cam = Camera.main;
        if (cam == null) yield break;

        Debug.Log("(>>>>>>>>>>>>>>>> Found Main Camera");
        Transform camTf = cam.transform;

        Vector3 pos = camTf.position + camTf.forward * 0.1f;
        pos.y = camTf.position.y;

        Debug.Log("(>>>>>>>>>>>>>>>> Translate Tutorial Canvas");
        tutorialgCanvas.transform.position = pos;

        tutorialgCanvas.transform.LookAt(camTf);
        tutorialgCanvas.transform.Rotate(0, 180f, 0f);
    }

    public void StartInteraction(int step)
    {
        StopAllCoroutines();
        StartCoroutine(WaitForInteraction(step));
    }

    private IEnumerator WaitForInteraction(int step)
    {
        isWaitingInteraction = true;
        DescriptionTxt.text = dialogueSteps[step].desc;

        // Step별 오브젝트 제어
        SetObjectsForStep(step);

        // 제스처 인식 대기
        while (isWaitingInteraction)
        {
            yield return null;
            if (StateManager.instance.interactionState == dialogueSteps[step].state)
            {
                isWaitingInteraction = false;
            }
        }

        DescriptionTxt.text = dialogueSteps[step].result;

        yield return new WaitForSeconds(3f);

        if (step < dialogueSteps.Count - 1)
        {
            StartInteraction(step + 1);
        }
        else
        {
            DescriptionTxt.text = "이제 여러분의 새를 만나보러 가볼까요!";
            yield return new WaitForSeconds(1f);
            SceneLoader.Instance.LoadScene(2);
        }
    }


    private void SetObjectsForStep(int step)
    {
        for (int i = 0; i < objectsToToggle.Count; i++)
        {
            if (objectsToToggle[i] != null)
            {
                objectsToToggle[i].SetActive(i == step);  // 해당 step만 true
            }
        }
    }



    public void OnCallButtonClicked() => StartInteraction(0);
    public void OnFingerButtonClicked() => StartInteraction(1);
    public void OnPalmButtonClicked() => StartInteraction(2);
    public void OnPetButtonClicked() => StartInteraction(3);
    public void OnFollowButtonClicked() => StartInteraction(4);
    public void OnByeButtonClicked() => StartInteraction(5);
    public void OnTempButtonClicked() => StateManager.instance.SetInteractionState(InteractionState.Bye);
}
