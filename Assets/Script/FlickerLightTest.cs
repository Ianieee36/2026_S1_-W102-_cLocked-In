using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLightTest : MonoBehaviour
{
    public Light2D lightSource;

    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        lightSource = GetComponent<Light2D>();

        bool flickerOn = PlayerPrefs.GetInt("FlickeringLights", 1) == 1;
        SetFlickerEnabled(flickerOn);
    }

    void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        lightSource.intensity = randomIntensity;
    }

    public void SetFlickerEnabled(bool isOn)
    {
        if (lightSource == null)
            lightSource = GetComponent<Light2D>();

        CancelInvoke("Flicker");

        if (isOn)
        {
            enabled = true;
            lightSource.enabled = true;
            InvokeRepeating("Flicker", 0, flickerSpeed);
        }
        else
        {
            enabled = false;

            // Choose ONE option:

            // Option A: turn light completely OFF
            lightSource.enabled = false;

            // Option B: keep light ON but stop flickering
            // lightSource.enabled = true;
            // lightSource.intensity = maxIntensity;
        }
    }
}