using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Setting")]
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
        StartTutorial();
    }

    public void StartTutorial()
    {
        StartInteraction(0);
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
        }
    }

    public void OnCallButtonClicked() => StartInteraction(0);
    public void OnFingerButtonClicked() => StartInteraction(1);
    public void OnPalmButtonClicked() => StartInteraction(2);
    public void OnPetButtonClicked() => StartInteraction(3);
    public void OnFollowButtonClicked() => StartInteraction(4);
    public void OnByeButtonClicked() => StartInteraction(5);
}
