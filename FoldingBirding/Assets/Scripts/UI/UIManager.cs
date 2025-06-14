using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Transform centerEyeAnchor;
    [SerializeField] private Transform UIObject;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TMP_Text dialogueTxt;

    [Header("Value")]
    [SerializeField] private float forwardOffset;
    [SerializeField] private float upOffset;

    private (string dialogue, float delay)[] dialogues = new (string, float)[]
    {
        ("폴딩버딩에 오신 걸 환영합니다.", 4f),
        ("종이새와 함께 MR체험을 즐길 수 있는 공간에 지금 막 도착했어요.", 5f),
        ("여러분의 종이새를 만나기 전에 간단한 튜토리얼을 진행해보겠습니다!", 5f),
    };

    // Start is called before the first frame update
    void Start()
    {
        UIObject.position = centerEyeAnchor.position + centerEyeAnchor.forward * forwardOffset + centerEyeAnchor.up * upOffset;
    }

    public void PressStartBtn()
    {
        startUI.SetActive(false);
        StartCoroutine(ShowDialogue());
    }

/*    private void MoveUI()
    {
        Vector3 targerPos = centerEyeAnchor.position + centerEyeAnchor.forward * forwardOffset - centerEyeAnchor.up * downOffset;
            UIObject.position = Vector3.Lerp(UIObject.position, targerPos, Time.deltaTime * velocity);

            UIObject.LookAt(centerEyeAnchor);
            UIObject.transform.Rotate(0, 180, 0);
    }*/

    private IEnumerator ShowDialogue()
    {
        dialogueUI.SetActive(true);
        foreach (var dg in dialogues)
        {
            dialogueTxt.text = dg.dialogue;
            yield return StartCoroutine(FadeEffect(1f, 0.5f)); 

            yield return new WaitForSeconds(dg.delay);

            yield return StartCoroutine(FadeEffect(0f, 0.5f));
        }
        yield return StartCoroutine(FadeEffect(0f, 0.5f));
        dialogueUI.SetActive(false);   
    }

    private IEnumerator FadeEffect(float resAlpha, float duration)
    {
        var color = dialogueTxt.color;
        float initAlpha = color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;  
            float curAlpha = Mathf.Lerp(initAlpha, resAlpha, t);
            dialogueTxt.color = new UnityEngine.Color(color.r, color.g, color.b, curAlpha);
            yield return null;
        }

        dialogueTxt.color = new UnityEngine.Color(color.r, color.g, color.b, resAlpha);
    }
}
