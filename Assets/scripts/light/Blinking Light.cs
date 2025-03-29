using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private Light lightSource;
    public float minBlinkTime = 0.2f;
    public float maxBlinkTime = 1.5f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public bool enableBlinking = true;
    public bool enableIntensityChange = true;

    void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            if (enableBlinking)
            {
                lightSource.enabled = !lightSource.enabled;
            }

            if (enableIntensityChange)
            {
                lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            }

            yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));
        }
    }
}