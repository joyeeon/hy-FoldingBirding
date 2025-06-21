using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class AnchorLoader : MonoBehaviour
{
    private OVRSpatialAnchor anchorPrefab;
    private SpatialAnchorManager spatialAnchorManager;

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
            StorageLocation = OVRSpace.StorageLocation.Local,
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
        if (!success)
        {
            Debug.LogWarning($"[LOAD] Failed to localize anchor: {unboundAnchor.Uuid}");
            return;
        }

        var pose = unboundAnchor.Pose;
        var spatialAnchor = Instantiate(anchorPrefab, pose.position, pose.rotation);
        unboundAnchor.BindTo(spatialAnchor);

        if (spatialAnchor.TryGetComponent<OVRSpatialAnchor>(out var anchor))
        {
            var textComponents = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>();
            if (textComponents.Length >= 2)
            {
                textComponents[0].text = "UUID: " + spatialAnchor.Uuid.ToString();
                textComponents[1].text = "Loaded from Device";
            }
        }

        Debug.Log($"[LOAD] Anchor successfully instantiated and bound: {spatialAnchor.Uuid}");
    }
}
