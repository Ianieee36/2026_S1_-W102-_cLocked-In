using UnityEngine;
using UnityEngine.UI;

public class GenderSettingsManager : MonoBehaviour
{
    public Toggle girlToggle;

    private const string GenderKey = "PlayerGender";

    private void Start()
    {
        string savedGender = PlayerPrefs.GetString(GenderKey, "Boy");

        girlToggle.isOn = savedGender == "Girl";

        girlToggle.onValueChanged.AddListener(SetGender);
    }

    public void SetGender(bool isGirl)
    {
        PlayerPrefs.SetString(GenderKey, isGirl ? "Girl" : "Boy");
        PlayerPrefs.Save();
    }
}