using UnityEngine;
using UnityEngine.Rendering.Universal;
public class FlickerLightTest : MonoBehaviour   
{
    public Light2D lightSource;
    //Settings below are changeable
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        //Gets the Light2D component attached to the same GameObject
        lightSource = GetComponent<Light2D>();
        //Starts the flickering effect by repeatedly calling the Flicker method
        InvokeRepeating("Flicker", 0, flickerSpeed);
    }

    void Flicker()
    {
        //Randomizes the intensity of light with the set minimum and maximum intensities
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        lightSource.intensity = randomIntensity;
    }
}
