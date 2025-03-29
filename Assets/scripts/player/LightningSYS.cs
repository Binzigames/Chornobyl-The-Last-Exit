using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashlightSystem : MonoBehaviour
{
    public Slider batterySlider;
    public CanvasGroup sliderCanvasGroup;
    public float maxBattery = 100f;
    public float batteryDrainSpeed = 10f;
    public float batteryRechargeSpeed = 5f;
    public float fadeSpeed = 2f;

    private Light flashlight;
    private float currentBattery;
    private bool isOn = false;
    private Coroutine fadeRoutine;
    private Coroutine sliderFadeRoutine;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
            enabled = false;
            return;
        }

        flashlight = player.GetComponentInChildren<Light>();
        if (flashlight == null)
        {
            Debug.LogError("Flashlight not found! Make sure there is a Light component in the player's hierarchy.");
            enabled = false;
            return;
        }

        flashlight.enabled = false;
        flashlight.intensity = 0;
        currentBattery = maxBattery;
        batterySlider.maxValue = maxBattery;
        batterySlider.value = currentBattery;

        if (sliderCanvasGroup != null)
        {
            sliderCanvasGroup.alpha = 0f;
        }
    }

    void Update()
    {
        HandleFlashlightToggle();
        UpdateBattery();
        UpdateSliderVisibility();
    }

    private void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentBattery > 0)
        {
            isOn = !isOn;
            flashlight.enabled = isOn;

            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeLight(isOn ? 1f : 0f));
        }
    }

    private void UpdateBattery()
    {
        if (isOn)
        {
            currentBattery -= batteryDrainSpeed * Time.deltaTime;
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                isOn = false;
                flashlight.enabled = false;
                if (fadeRoutine != null) StopCoroutine(fadeRoutine);
                fadeRoutine = StartCoroutine(FadeLight(0f));
            }
        }
        else if (currentBattery < maxBattery)
        {
            currentBattery += batteryRechargeSpeed * Time.deltaTime;
            currentBattery = Mathf.Min(currentBattery, maxBattery);
        }
        batterySlider.value = currentBattery;
    }

    private void UpdateSliderVisibility()
    {
        if (sliderCanvasGroup == null) return;

        bool shouldBeVisible = currentBattery < maxBattery;
        if (shouldBeVisible && sliderCanvasGroup.alpha < 1f)
        {
            if (sliderFadeRoutine != null) StopCoroutine(sliderFadeRoutine);
            sliderFadeRoutine = StartCoroutine(FadeSlider(1f));
        }
        else if (!shouldBeVisible && sliderCanvasGroup.alpha > 0f)
        {
            if (sliderFadeRoutine != null) StopCoroutine(sliderFadeRoutine);
            sliderFadeRoutine = StartCoroutine(FadeSlider(0f));
        }
    }

    IEnumerator FadeLight(float targetIntensity)
    {
        float startIntensity = flashlight.intensity;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            flashlight.intensity = Mathf.Lerp(startIntensity, targetIntensity, time);
            yield return null;
        }
        flashlight.intensity = targetIntensity;
    }

    IEnumerator FadeSlider(float targetAlpha)
    {
        float startAlpha = sliderCanvasGroup.alpha;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            sliderCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time);
            yield return null;
        }
        sliderCanvasGroup.alpha = targetAlpha;
    }
}