using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TransitionManager : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Transform CameraRig;
    [SerializeField] private Transform UIObject;
    [SerializeField] private GameObject displayQuad;
    [SerializeField] private Material displayMat;
    [SerializeField] private TMP_Text transitionTxt1;
    [SerializeField] private TMP_Text transitionTxt2;
    [SerializeField] private Light light;

    [Header("Value")]
    [SerializeField] private float forwardOffset;
    [SerializeField] private float upOffset;


    // Start is called before the first frame update
    void Start()
    {
        displayQuad.SetActive(false);
        UIObject.position = CameraRig.position + CameraRig.forward * forwardOffset + CameraRig.up * upOffset;
    }

    private IEnumerator FadeEffect(float targetAlpha, float duration)
    {
        Color quadColor = displayMat.color;
        Color txt1Color = transitionTxt1.color;
        Color txt2Color = transitionTxt2.color;

        float initAlpha = quadColor.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float curAlpha = Mathf.Lerp(initAlpha, targetAlpha, t);

            displayMat.color = new Color(quadColor.r, quadColor.g, quadColor.b, curAlpha);

            transitionTxt1.color = new Color(txt1Color.r, txt1Color.g, txt1Color.b, curAlpha);
            transitionTxt2.color = new Color(txt2Color.r, txt2Color.g, txt2Color.b, curAlpha);

            light.intensity = 1 - curAlpha;  

            yield return null;
        }

        displayMat.color = new Color(quadColor.r, quadColor.g, quadColor.b, targetAlpha);
        transitionTxt1.color = new Color(txt1Color.r, txt1Color.g, txt1Color.b, targetAlpha);
        transitionTxt2.color = new Color(txt2Color.r, txt2Color.g, txt2Color.b, targetAlpha);
    }

    private IEnumerator TransitionScreen()
    {
        displayQuad.SetActive(true);
        StartCoroutine(FadeEffect(1f, 0.7f));

        var delay = 7f;
        yield return new WaitForSeconds(delay);

        StartCoroutine(FadeEffect(0f, 0.7f));
        //displayQuad.SetActive(false);
    }

    public void PressSelectBtn()
    {
        StartCoroutine(TransitionScreen());
    }
}
