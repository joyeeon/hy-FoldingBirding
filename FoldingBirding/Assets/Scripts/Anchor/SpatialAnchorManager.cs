using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpatialAnchorManager : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;
    public const string NumUuidsPlayerPref = "numUuids";

    private Canvas canvas;
    private TextMeshProUGUI uuidText;
    private TextMeshProUGUI savedStatusText;
    private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
    private OVRSpatialAnchor lastCreatedAnchor;
    public AnchorLoader anchorLoader;



    void Start(){
        Debug.Log($"[APPID] Application.identifier" + Application.identifier);
    }
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }
        if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            SaveLastCreateAnchor();
        }
        if(OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            UnsavedLastCreatedAnchor();
        }
        if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log($"[CLICKED] 리셋 클릭됨");
            UnsaveAllAnchors();
        }
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            Debug.Log($"[CLICKED] 로드 클릭됨");
            LoadSavedAnchors();
        }
    }

    public void CreateSpatialAnchor()
    {
        OVRSpatialAnchor workingAnchor = Instantiate(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),  OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

        Debug.Log($"[CLICKED] workingAnchor");

        canvas = workingAnchor.gameObject.GetComponentInChildren<Canvas>();
        uuidText = canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        savedStatusText = canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
    {
        while(!workingAnchor.Created && !workingAnchor.Localized)
        {
            Debug.Log($"[CLICKED] 기다림?");
            yield return new WaitForEndOfFrame();
        }
        Guid anchorGuid = workingAnchor.Uuid;
        anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;
        uuidText.text = "UUID: " + anchorGuid.ToString();
        savedStatusText.text = "Not Saved";
    }

    private async void SaveLastCreateAnchor()
    {
        bool success = await lastCreatedAnchor.SaveAnchorAsync();

        Debug.Log("[SAVED] Anchor Save Success: " + success);

        if (success)
        {
            savedStatusText.text = "Saved";
            SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
            Debug.Log($"[SAVED] Anchor Saved Completed");
        }
    }


    void SaveUuidToPlayerPrefs(Guid uuid)
{
    if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
    {
        PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
    }

    int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);

    // ✅ 중복 UUID 검사 먼저 수행
    for (int i = 0; i < playerNumUuids; i++)
    {
        string existingUuid = PlayerPrefs.GetString("uuid" + i);
        if (existingUuid == uuid.ToString())
        {
            Debug.Log("[UUID] Duplicate UUID detected, skipping save: " + uuid);
            return;
        }
    }

    // ✅ 중복이 아니라면 저장
    PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
    PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
    PlayerPrefs.Save();

    Debug.Log("[UUID] Saved new Anchor UUID: " + uuid);

    // 디버그 출력 (검사용)
    int count = PlayerPrefs.GetInt(NumUuidsPlayerPref, 0);
    Debug.Log("[UUID] Total Saved Count: " + count);
    for (int i = 0; i < count; i++)
    {
        string uuidString = PlayerPrefs.GetString("uuid" + i);
        Debug.Log($"[UUID] PlayerPrefs UUID[{i}] = {uuidString}");
        try
        {
            var guid = new Guid(uuidString);
            Debug.Log($"[UUID] Parsed Guid: {guid}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[UUID] Failed to parse UUID: {uuidString} => {e.Message}");
        }
    }
}


    private void UnsavedLastCreatedAnchor()
    {
        lastCreatedAnchor.Erase((lastCreatedAnchor, success) => 
        {
            if (success)
            {
                savedStatusText.text = "Not Saved";
            }
        });
    }

    private void UnsaveAllAnchors()
    {
        foreach (var anchor in anchors)
        {
            UnsaveAnchor(anchor);
        }
        anchors.Clear();
        ClearAllUuidsFromPlayerPrefs();
    }

    private void UnsaveAnchor(OVRSpatialAnchor anchor)
    {
        anchor.Erase((erasedAnchor, success) => 
        {
            if(success)
            {
                var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
                if(textComponents.Length > 1)
                {
                    var savedStatusText = textComponents[1];
                    savedStatusText.text = "Not Saved";
                }
            }
        });
    }

    private void ClearAllUuidsFromPlayerPrefs()
    {
        if(PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
            for(int i = 0; i < playerNumUuids; i++)
            {
                PlayerPrefs.DeleteKey("uuid" + i);
            }
            PlayerPrefs.DeleteKey(NumUuidsPlayerPref);
            PlayerPrefs.Save();
        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All saved anchors have been cleared from PlayerPrefs.");
    }

    public void LoadSavedAnchors()
    {
        anchorLoader.LoadAnchorsByUuid();
    }
    
}
