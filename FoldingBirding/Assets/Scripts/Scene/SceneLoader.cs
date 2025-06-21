using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private GameObject transitionPanel;

    private GameObject activePanel;
    private bool isTransitioning = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneId)
    {
        if (isTransitioning) return;

        string sceneName = sceneId switch
        {
            0 => "1. Opening",
            1 => "2. Interaction Tutorial",
            2 => "3. Select Mat",
            3 => "4. MR Space",
            _ => null
        };

        if (sceneName == null)
        {
            Debug.LogError("Scene ID not assigned.");
            return;
        }
        isTransitioning = true;

        SetPanel(sceneId);
        StartCoroutine(TransitionToScene(sceneName));
    }


    private void SetPanel(int sceneId)
    {
        if (activePanel == null)
        {
            activePanel = Instantiate(transitionPanel);
            DontDestroyOnLoad(activePanel);
        }

        if (sceneId == 2)
        {
            activePanel.transform.GetChild(1).gameObject.SetActive(true);
            activePanel.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (sceneId == 3)
        {
            activePanel.transform.GetChild(1).gameObject.SetActive(false);
            activePanel.transform.GetChild(2).gameObject.SetActive(true);
        }

        Camera cam = Camera.main;
        Debug.Log(cam.gameObject.name);
        if (cam != null)
        {
            Transform camTf = cam.transform;
            Vector3 pos = camTf.position + camTf.forward * 0.4f;
            activePanel.transform.position = pos;

            activePanel.transform.LookAt(camTf);
            activePanel.transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        activePanel?.SetActive(true);
        CanvasGroup canvasGroup = activePanel.GetComponent<CanvasGroup>();

        yield return FadePanel(canvasGroup, 0f, 1f, 1f);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(RepositionPanel());
        yield return FadePanel(canvasGroup, 1f, 0f, 1f);

        isTransitioning = false;
    }

    private IEnumerator FadePanel(CanvasGroup canvasGroup ,float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, timer / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
    private IEnumerator RepositionPanel()
    {
        Camera cam = null;
        while ((cam = Camera.main) == null )
        {
            yield return null;
        }

        Transform camTf = cam.transform;
        Vector3 pos = camTf.position + camTf.forward * 0.4f;
        activePanel.transform.position = pos;
        activePanel.transform.LookAt(camTf);
        activePanel.transform.Rotate(0, 180, 0);
    }

    public void LoadSelectInteractionTutorial() => SceneLoader.Instance.LoadScene(1);
    public void LoadSelectMetScene() => SceneLoader.Instance.LoadScene(2);
}