using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class AnchorLoader : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;
    private SpatialAnchorManager spatialAnchorManager;

    private void Start()
    {
        if (spatialAnchorManager == null)
            spatialAnchorManager = FindObjectOfType<SpatialAnchorManager>();

        if (spatialAnchorManager != null)
        {
            anchorPrefab = spatialAnchorManager.anchorPrefab;
            if (anchorPrefab == null)
            {
                Debug.LogError("[LOAD] anchorPrefab is still null in Start()");
            }
        }
        LoadAnchorsByUuid();
        
    }
    private void Awake()
    {
        spatialAnchorManager = GetComponent<SpatialAnchorManager>();
        anchorPrefab = spatialAnchorManager.anchorPrefab;
    }

    public void LoadAnchorsByUuid()
    {
        if (!PlayerPrefs.HasKey(SpatialAnchorManager.NumUuidsPlayerPref))
        {
            Debug.LogWarning("[LOAD] No saved UUIDs found.");
            return;
        }

        int playerUuidCount = PlayerPrefs.GetInt(SpatialAnchorManager.NumUuidsPlayerPref);
        if (playerUuidCount == 0)
        {
            Debug.LogWarning("[LOAD] UUID count is 0.");
            return;
        }

        List<Guid> uuids = new List<Guid>();
        for (int i = 0; i < playerUuidCount; i++)
        {
            string uuidKey = "uuid" + i;
            string currentUuid = PlayerPrefs.GetString(uuidKey);

            if (Guid.TryParse(currentUuid, out var parsedGuid))
            {
                if (!uuids.Contains(parsedGuid))
                {
                    uuids.Add(parsedGuid);
                    Debug.Log($"[LOAD] Valid UUID[{i}]: {parsedGuid}");
                }
                else
                {
                    Debug.LogWarning($"[LOAD] Duplicate UUID skipped: {parsedGuid}");
                }
            }
            else
            {
                Debug.LogError($"[LOAD] Failed to parse UUID[{i}]: {currentUuid}");
            }
        }

        if (uuids == null)
        {
            Debug.LogWarning("[LOAD] No valid UUIDs to load.");
            return;
        }

        _ = LoadAnchorsFromGuids(uuids);
    }

    private async Task LoadAnchorsFromGuids(List<Guid> uuids)
    {
        Debug.Log($"[LOAD] Attempting to load {uuids.Count} anchors...");

        var loadOptions = new OVRSpatialAnchor.LoadOptions
        {
            StorageLocation = OVRSpace.StorageLocation.Cloud,
            Timeout = 0,
            Uuids = uuids
        };

        var anchors = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(loadOptions);

        if (anchors == null)
        {
            Debug.LogWarning("[LOAD] No anchors were returned from LoadUnboundAnchorsAsync.");
            return;
        }

        Debug.Log($"[LOAD] Returned {anchors} anchors from LoadUnboundAnchorsAsync.");

        foreach (var unboundAnchor in anchors)
        {
            Debug.Log($"[LOAD] Anchor UUID: {unboundAnchor.Uuid}, Localized: {unboundAnchor.Localized}");

            bool success = unboundAnchor.Localized;

            if (!success)
            {
                success = await unboundAnchor.LocalizeAsync();
                Debug.Log($"[LOAD] LocalizeAsync success: {success}");
            }

            OnLocalized(unboundAnchor, success);
        }
    }

    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        Debug.Log($"[LOAD] 이거 되긴 하냐? ");
        if (!success)
        {
            Debug.LogWarning($"[LOAD] Failed to localize anchor: {unboundAnchor.Uuid}");
            return;
        }

        if (anchorPrefab == null)
        {
            Debug.LogError("[LOAD] anchorPrefab is null. Cannot instantiate anchor.");
            return;
        }

        if (unboundAnchor.TryGetPose(out Pose pose))
        {
            var spatialAnchor = Instantiate(anchorPrefab, pose.position, pose.rotation);
            unboundAnchor.BindTo(spatialAnchor);

            var uuidText = spatialAnchor.transform.Find("Canvas/UUIDText")?.GetComponent<TextMeshProUGUI>();
            var statusText = spatialAnchor.transform.Find("Canvas/StatusText")?.GetComponent<TextMeshProUGUI>();

            if (uuidText != null && statusText != null)
            {
                uuidText.text = "UUID: " + spatialAnchor.Uuid.ToString();
                statusText.text = "Loaded from Device";
            }

            Debug.Log($"[LOAD] Anchor successfully instantiated and bound: {spatialAnchor.Uuid}");
        }
        else
        {
            Debug.LogError($"[LOAD] Failed to get pose from unbound anchor: {unboundAnchor.Uuid}");
        }
    }
}
