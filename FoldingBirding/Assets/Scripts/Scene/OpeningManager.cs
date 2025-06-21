using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TMP_Text dialogueTxt;
    [SerializeField] private GameObject openingCanvas;

    [Header("Value")]
    [SerializeField] private float forwardOffset;
    [SerializeField] private float upOffset;

    private (string dialogue, float delay)[] dialogues = new (string, float)[]
    {
        ("���������� ���� �� ȯ���մϴ�.", 4f),
        ("���̻��� �Բ� MRü���� ��� �� �ִ� ������ ���� �� �����߾��.", 5f),
        ("�������� ���̻��� ������ ���� ������ Ʃ�丮���� �����غ��ڽ��ϴ�!", 5f),
    };

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(SetCanvas());
    }

    private IEnumerator SetCanvas()
    {
        yield return new WaitForSeconds(0.3f); 

        Camera cam = Camera.main;
        if (cam == null) yield break;

        Transform camTf = cam.transform;

        Vector3 pos = camTf.position + camTf.forward * 0.1f;
        pos.y = camTf.position.y;

        openingCanvas.transform.position = pos;

        openingCanvas.transform.LookAt(camTf);
        openingCanvas.transform.Rotate(0, 180f, 0f); 
    }


    public void PressStartBtn()
    {
        startUI.SetActive(false);
        StartCoroutine(ShowDialogue());
    }

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
        SceneLoader.Instance.LoadScene(1);
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
