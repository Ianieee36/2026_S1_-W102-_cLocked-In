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
            InvokeRepeating("Flicker", 0, flickerSpeed);
    }

    void Flicker()
    {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            lightSource.intensity = randomIntensity;
    }
}
