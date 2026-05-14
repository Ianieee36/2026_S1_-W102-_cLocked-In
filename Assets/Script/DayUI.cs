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
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (DayManager.Instance == null) return;

        // This was missing!
        dayCounterText.text = "Day " + DayManager.Instance.currentDay;

        // Map current time to 9AM - 5PM
        float startHour = 9f;
        float endHour = 17f;
        float totalHours = endHour - startHour;
        float progress = DayManager.Instance.currentTime / DayManager.Instance.dayLengthInSeconds;
        float currentHour = startHour + progress * totalHours;

        int hours = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);
        string period = hours >= 12 ? "PM" : "AM";
        int displayHour = hours > 12 ? hours - 12 : hours;
        timeCounterText.text = string.Format("{0}:{1:00} {2}", displayHour, minutes, period);
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

        CanvasGroup cg = dayPopup.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        // Hold for a bit then fade out
        yield return new WaitForSeconds(popupDuration);

        float fadeDuration = 1f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        dayPopup.SetActive(false);
    }
}