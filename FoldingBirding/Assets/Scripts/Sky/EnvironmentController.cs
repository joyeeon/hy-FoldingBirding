using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public enum TimeOfDay { Day, Sunset, Night }

    [Header("Skybox Material (NoFogForUnlit)")]
    public Material skyboxMaterial;

    [Header("Directional Light")]
    public Light directionalLight;

    [Header("Night Light Objects")]
    public GameObject[] nightLights;

    [Header("Play Time (초)")]
    public float totalPlayTime = 300f;

    private float timer = 0f;
    private TimeOfDay currentTime;

    // 목표값
    private Color targetTopColor;
    private Color targetMiddleColor;
    private Color targetFogColor;
    private Vector3 targetLightRotation;
    private bool nightActive;

    // 현재값
    private Color currentTopColor;
    private Color currentMiddleColor;
    private Color currentFogColor;
    private Vector3 currentLightRotation;

    void Start()
    {
        SetEnvironment(TimeOfDay.Day, immediate: true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 120f && currentTime != TimeOfDay.Day)
            SetEnvironment(TimeOfDay.Day);
        else if (timer >= 120f && timer < 240f && currentTime != TimeOfDay.Sunset)
            SetEnvironment(TimeOfDay.Sunset);
        else if (timer >= 240f && currentTime != TimeOfDay.Night)
            SetEnvironment(TimeOfDay.Night);

        // Lerp 적용
        float lerpSpeed = 0.5f * Time.deltaTime;

        currentTopColor = Color.Lerp(currentTopColor, targetTopColor, lerpSpeed);
        currentMiddleColor = Color.Lerp(currentMiddleColor, targetMiddleColor, lerpSpeed);
        currentFogColor = Color.Lerp(currentFogColor, targetFogColor, lerpSpeed);
        currentLightRotation = Vector3.Lerp(currentLightRotation, targetLightRotation, lerpSpeed);

        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetColor("_TopSkyColor", currentTopColor);
            skyboxMaterial.SetColor("_MiddleSkyColor", currentMiddleColor);
        }

        RenderSettings.fogColor = currentFogColor;

        if (directionalLight != null)
            directionalLight.transform.eulerAngles = currentLightRotation;
    }

    public void SetEnvironment(TimeOfDay time, bool immediate = false)
    {
        currentTime = time;

        switch (time)
        {
            case TimeOfDay.Day:
                SetTarget("#688DFD", "#BBDADD", "#B3D7DB", new Vector3(80, -500, -50), false, immediate);
                break;
            case TimeOfDay.Sunset:
                SetTarget("#95ACE9", "#FFB25A", "#BEAA99", new Vector3(17, -452, -8), false, immediate);
                break;
            case TimeOfDay.Night:
                SetTarget("#3D4565", "#000535", "#324C59", new Vector3(0, -520, -20), true, immediate);
                break;
        }
    }

    private void SetTarget(string topHex, string middleHex, string fogHex, Vector3 rotation, bool isNight, bool immediate)
    {
        targetTopColor = HexToColor(topHex);
        targetMiddleColor = HexToColor(middleHex);
        targetFogColor = HexToColor(fogHex);
        targetLightRotation = rotation;
        nightActive = isNight;

        if (immediate)
        {
            currentTopColor = targetTopColor;
            currentMiddleColor = targetMiddleColor;
            currentFogColor = targetFogColor;
            currentLightRotation = targetLightRotation;
        }

        foreach (GameObject obj in nightLights)
        {
            if (obj != null)
                obj.SetActive(isNight);
        }
    }

    private static Color HexToColor(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}
