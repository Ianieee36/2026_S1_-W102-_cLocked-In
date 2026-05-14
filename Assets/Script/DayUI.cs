using UnityEngine;
using TMPro;
using System.Collections;

public class DayUI : MonoBehaviour
{
    public static DayUI Instance;

    [Header("UI References")]
    public TextMeshProUGUI dayCounterText;
    public TextMeshProUGUI timeCounterText;
    public GameObject dayPopup;
    public TextMeshProUGUI dayPopupText;

    [Header("Popup Settings")]
    public float popupDuration = 3f; // How long the popup stays on screen

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if(DayManager.Instance == null) return;

        // Update day counter
        dayCounterText.text = "Day " + DayManager.Instance.currentDay;

        // Update time counter - formats as MM:SS
        float t = DayManager.Instance.currentTime;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        timeCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowNewDay(int day)
    {
        StopAllCoroutines();
        StartCoroutine(ShowDayPopup(day));
    }

    IEnumerator ShowDayPopup(int day)
    {
        dayPopupText.text = "Day " + day;
        dayPopup.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        dayPopup.SetActive(false);
    }
}