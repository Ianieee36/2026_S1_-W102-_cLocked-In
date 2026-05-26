using UnityEngine;
using UnityEngine.UI;

public class FlickerSettingsManager : MonoBehaviour
{
    public Toggle flickerToggle;

    private const string FlickerKey = "FlickeringLights";

    private void Start()
    {
        bool flickerOn = PlayerPrefs.GetInt(FlickerKey, 1) == 1;

        flickerToggle.isOn = flickerOn;

        ApplyFlickerSetting(flickerOn);

        flickerToggle.onValueChanged.AddListener(ApplyFlickerSetting);
    }

    public void ApplyFlickerSetting(bool isOn)
    {
        PlayerPrefs.SetInt(FlickerKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        FlickerLightTest[] lights =
            FindObjectsOfType<FlickerLightTest>();

        foreach (FlickerLightTest light in lights)
        {
            light.SetFlickerEnabled(isOn);
        }
    }
}