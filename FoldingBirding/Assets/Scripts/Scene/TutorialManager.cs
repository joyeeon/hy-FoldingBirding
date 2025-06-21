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


    private List<(StateManager.InteractionState state, string desc, string result)> dialogueSteps = new()
    {
        (StateManager.InteractionState.Call,   "���� ���� ���� �ҷ� ������!", "����� �θ��� ���� �����߾��!"),
        (StateManager.InteractionState.Finger, "�հ��� �ϳ��� ��! ���� ���� �ڸ��� ������ּ���!", "���� ����� �հ��� ���� �ɾҾ��!"),
        (StateManager.InteractionState.Palm,   "�չٴ��� ���ĺ�����.", "����� ���� �����Դ� ���� ����� �ڸ�����."),
        (StateManager.InteractionState.Pet,    "���� �Ӹ��� ���ɽ��� ���ٵ�����.", "���� �⻵�ϰ� �־��! ��Ű� �� ���̿� �ŷڰ� ������."),
        (StateManager.InteractionState.Follow, "�Բ� �ɾ���? ���߸� �����ؿ�!", "�Բ� �Ȱ� �־��. ����� �翡�� �ȴ� �� ���� �����ϴ� �� ���ƿ�."),
        (StateManager.InteractionState.Bye,    "�ٽ� ������ ���� ����ϸ� ���� ���� ������.", "���� �λ縦 ����� �־������. ������ ����, ����� �ּ���!"),
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

        while (isWaitingInteraction)
        {
            yield return null;
            if (StateManager.instance.interactionState == dialogueSteps[step].state)
            {
                isWaitingInteraction = false;
            }
        }

        DescriptionTxt.text = dialogueSteps[step].result;

        var delay = 3f;
        yield return new WaitForSeconds(delay);

        if(step < 5)
        {
            StartInteraction(step + 1);
        }
        else
        {
            DescriptionTxt.text = "���� �������� ���� �������� �������!";

            delay = 1f;
            yield return new WaitForSeconds(delay);

            SceneLoader.Instance.LoadScene(2);
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
