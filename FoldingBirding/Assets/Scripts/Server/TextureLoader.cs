using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TextureLoader : MonoBehaviour
{
    public string serverUrl = "http://localhost:8000"; // FastAPI 서버 주소
    public Transform scrollParent; // 닉네임 버튼이 붙을 부모 오브젝트
    public GameObject nicknameButtonPrefab;
    public Material targetMaterial;

    void Start()
    {
        StartCoroutine(FetchTextureList());
    }

    IEnumerator FetchTextureList()
    {
        string url = $"{serverUrl}/textures";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("? Failed to fetch texture list");
            yield break;
        }

        string json = req.downloadHandler.text;
        TextureListWrapper data = JsonUtility.FromJson<TextureListWrapper>(json);

        foreach (string name in data.nicknames)
        {
            GameObject btn = Instantiate(nicknameButtonPrefab, scrollParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = name;
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartCoroutine(DownloadTexture(name)));
        }
    }

    IEnumerator DownloadTexture(string nickname)
    {
        string url = $"{serverUrl}/textures/{nickname}";
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"? Failed to download texture for {nickname}");
            yield break;
        }

        Texture2D tex = DownloadHandlerTexture.GetContent(req);
        targetMaterial.mainTexture = tex;
        Debug.Log($"? {nickname} 텍스처 적용 완료");
    }

    [System.Serializable]
    public class TextureListWrapper
    {
        public List<string> nicknames;
    }
}