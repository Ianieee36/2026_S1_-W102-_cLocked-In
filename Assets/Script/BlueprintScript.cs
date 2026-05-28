using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BlueprintScript : Item
{
    [Header("Map World Bounds")]
    public Vector2 mapWorldMin;
    public Vector2 mapWorldMax;

    private Transform playerTransform;
    private Transform bossTransform;
    private RectTransform mapUI;
    private RectTransform playerPing;
    private RectTransform bossPing;
    private CanvasGroup mapCanvasGroup;
    private bool mapOpen = false;

    void FindReferences()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTransform = player.transform;
            else Debug.LogWarning("BlueprintScript: Player not found.");
        }

        if (bossTransform == null)
        {
            GameObject boss = GameObject.FindWithTag("BossCharacter");
            if (boss != null) bossTransform = boss.transform;
            else Debug.LogWarning("BlueprintScript: Boss not found.");
        }

        if (mapUI == null)
        {
            GameObject map = GameObject.FindWithTag("MapUI");
            if (map != null)
            {
                mapUI = map.GetComponent<RectTransform>();
                mapCanvasGroup = map.GetComponent<CanvasGroup>();
                Debug.Log("MapUI found: " + map.name);
                Debug.Log("CanvasGroup found: " + (mapCanvasGroup != null));
            }
            else Debug.LogWarning("BlueprintScript: MapUI not found.");
        }

        if (playerPing == null)
        {
            GameObject ping = GameObject.FindWithTag("PlayerPing");
            if (ping != null) playerPing = ping.GetComponent<RectTransform>();
            else Debug.LogWarning("BlueprintScript: PlayerPing not found.");
        }

        if (bossPing == null)
        {
            GameObject ping = GameObject.FindWithTag("BossPing");
            if (ping != null) bossPing = ping.GetComponent<RectTransform>();
            else Debug.LogWarning("BlueprintScript: BossPing not found.");
        }
    }

    void SetMapVisible(bool visible)
    {
        Debug.Log("SetMapVisible called: " + visible + " | CanvasGroup: " + (mapCanvasGroup != null));
        if (mapCanvasGroup != null)
        {
            mapCanvasGroup.alpha = visible ? 1 : 0;
            Debug.Log("Alpha set to: " + mapCanvasGroup.alpha);
        }
    }

    public override void UseItem()
    {
        FindReferences();

        mapOpen = !mapOpen;
        SetMapVisible(mapOpen);

        if (mapOpen)
            UpdatePings();
    }

    void Update()
    {
        if (!mapOpen) return;

        //if (Keyboard.current.cKey.wasPressedThisFrame)
        UpdatePings();
    }

    void UpdatePings()
    {
        if (playerTransform != null)
            SetPingPosition(playerPing, playerTransform.position);

        if (bossTransform != null)
            SetPingPosition(bossPing, bossTransform.position);
    }

    void SetPingPosition(RectTransform ping, Vector3 worldPos)
    {
        if (ping == null || mapUI == null) return;

        float normalizedX = Mathf.InverseLerp(mapWorldMin.x, mapWorldMax.x, worldPos.x);
        float normalizedY = Mathf.InverseLerp(mapWorldMin.y, mapWorldMax.y, worldPos.y);

        ping.anchorMin = new Vector2(normalizedX, normalizedY);
        ping.anchorMax = new Vector2(normalizedX, normalizedY);
        ping.anchoredPosition = Vector2.zero;

        ping.gameObject.SetActive(true);
    }
}