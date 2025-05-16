using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [Header("Radiation")]
    public float currentRadiation = 0f;
    public float maxRadiation = 100f;
    public Slider radiationSlider;
    private CanvasGroup sliderCanvasGroup;
    private RadiationZone[] radiationZones;

    [Header("Audio")]
    public AudioSource radiationAudio;
    public AudioSource screamerAudio;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float radiationVolume = 1f;
    [Range(0f, 1f)] public float screamerVolume = 1f;

    [Header("Ambient Audio")]
    public List<AudioClip> ambientClips = new List<AudioClip>();
    public AudioSource ambientAudioSource;
    [Range(0f, 1f)] public float ambientVolume = 1f;
    private float ambientDelay = 3f;

    [Header("Pause menu")]
    public bool InPause = false;
    public GameObject PauseMenu = null;
    private CanvasGroup pauseCanvasGroup;

    [Header("Death UI")]
    public GameObject GamePanel = null;
    public GameObject Screamer = null;
    public TextMeshProUGUI deathMessage;
    public float screamerDuration = 3f;

    [Header("Notebook UI")]
    public GameObject notebookUI;
    public TextMeshProUGUI notebookTextUI;
    private bool isNotebookOpen = false;

    private bool isDead = false;
    private LocalizationManager localizationManager;

    private void Start()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();

        if (radiationSlider == null)
        {
            Debug.LogError("RadiationSlider is not assigned in the Inspector.");
            return;
        }

        sliderCanvasGroup = radiationSlider.GetComponentInParent<CanvasGroup>();
        if (sliderCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not found on the parent of the RadiationSlider.");
            return;
        }

        radiationZones = FindObjectsOfType<RadiationZone>();
        if (radiationZones.Length == 0)
        {
            Debug.LogError("No RadiationZone objects found in the scene.");
        }

        sliderCanvasGroup.alpha = 0f;

        if (PauseMenu != null)
        {
            pauseCanvasGroup = PauseMenu.GetComponent<CanvasGroup>();
            if (pauseCanvasGroup == null)
            {
                Debug.LogError("PauseMenu does not have a CanvasGroup.");
            }
        }
        else
        {
            Debug.LogWarning("PauseMenu is not assigned.");
        }

        notebookUI.SetActive(false);

        if (radiationAudio != null)
        {
            radiationAudio.loop = true;
            radiationAudio.Stop();
        }

        if (ambientAudioSource != null)
        {
            ambientAudioSource.loop = false;
            ambientAudioSource.playOnAwake = false;
            StartCoroutine(PlayAmbientLoop());
        }

        UpdateAudioVolumes();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isNotebookOpen)
                CloseNotebookUI();
            else if (!InPause)
                PauseGame();
            else
                ResumeGame();
        }

        if (radiationSlider == null || sliderCanvasGroup == null || radiationZones.Length == 0)
            return;

        bool isInRadiationZone = false;

        foreach (RadiationZone zone in radiationZones)
        {
            if (zone.IsInRadiationZone())
            {
                isInRadiationZone = true;
                currentRadiation += Time.deltaTime * zone.radiationIncreaseRate;
            }
        }

        if (isInRadiationZone)
        {
            if (sliderCanvasGroup.alpha < 1f)
                StartCoroutine(FadeInSlider());

            if (radiationAudio != null && !radiationAudio.isPlaying)
                radiationAudio.Play();

            radiationSlider.value = currentRadiation / maxRadiation;
            currentRadiation = Mathf.Min(currentRadiation, maxRadiation);

            if (radiationAudio != null)
            {
                float radiationRatio = currentRadiation / maxRadiation;
                radiationAudio.pitch = Mathf.Lerp(1f, 2f, radiationRatio);
            }

            if (currentRadiation >= maxRadiation && !isDead)
                Die("NOW YOU ARE GLOWING LIKE A TORCH!");
        }
        else
        {
            if (sliderCanvasGroup.alpha > 0f)
                StartCoroutine(FadeOutSlider());

            if (radiationAudio != null && radiationAudio.isPlaying)
                radiationAudio.Stop();
        }

        if (radiationAudio != null)
        {
            radiationAudio.volume = radiationVolume * masterVolume * (InPause ? 0.2f : 1f);
        }
    }

    public void Die(string causeOfDeath)
    {
        if (isDead) return;

        isDead = true;
        GamePanel.SetActive(false);
        Screamer.SetActive(true);

        if (radiationAudio != null && radiationAudio.isPlaying)
            radiationAudio.Stop();

        if (screamerAudio != null)
        {
            screamerAudio.volume = screamerVolume * masterVolume;
            screamerAudio.Play();
        }

        if (deathMessage != null)
            deathMessage.text = causeOfDeath;

        StartCoroutine(HandleDeathScreen());
    }

    private IEnumerator HandleDeathScreen()
    {
        yield return new WaitForSeconds(screamerDuration);

        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;

        Screamer.SetActive(false);
        GamePanel.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeInSlider()
    {
        float time = 0f;
        while (sliderCanvasGroup.alpha < 1f)
        {
            time += Time.deltaTime;
            sliderCanvasGroup.alpha = Mathf.Lerp(sliderCanvasGroup.alpha, 1f, time / 0.5f);
            yield return null;
        }
        sliderCanvasGroup.alpha = 1f;
    }

    public void SetRadiationSliderVisibility(bool visible)
    {
        StopAllCoroutines();
        StartCoroutine(visible ? FadeInSlider() : FadeOutSlider());
    }

    private IEnumerator FadeOutSlider()
    {
        float time = 0f;
        while (sliderCanvasGroup.alpha > 0f)
        {
            time += Time.deltaTime;
            sliderCanvasGroup.alpha = Mathf.Lerp(sliderCanvasGroup.alpha, 0f, time / 0.5f);
            yield return null;
        }
        sliderCanvasGroup.alpha = 0f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        InPause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        InPause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowNotebookUI(string text, string ua_text)
    {
        notebookUI.SetActive(true);
        notebookTextUI.text = localizationManager != null && localizationManager.IsUkranian ? ua_text : text;

        isNotebookOpen = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNotebookUI()
    {
        notebookUI.SetActive(false);
        isNotebookOpen = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ========== Volume Setters ==========
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        UpdateAudioVolumes();
    }

    public void SetRadiationVolume(float value)
    {
        radiationVolume = Mathf.Clamp01(value);
        UpdateAudioVolumes();
    }

    public void SetScreamerVolume(float value)
    {
        screamerVolume = Mathf.Clamp01(value);
        UpdateAudioVolumes();
    }

    public void SetAmbientVolume(float value)
    {
        ambientVolume = Mathf.Clamp01(value);
        UpdateAudioVolumes();
    }

    private void UpdateAudioVolumes()
    {
        if (radiationAudio != null)
            radiationAudio.volume = radiationVolume * masterVolume * (InPause ? 0.2f : 1f);

        if (screamerAudio != null)
            screamerAudio.volume = screamerVolume * masterVolume;

        if (ambientAudioSource != null)
            ambientAudioSource.volume = ambientVolume * masterVolume;
    }

    private IEnumerator PlayAmbientLoop()
    {
        while (true)
        {
            if (ambientClips.Count > 0)
            {
                AudioClip clip = ambientClips[Random.Range(0, ambientClips.Count)];
                ambientAudioSource.clip = clip;
                ambientAudioSource.volume = ambientVolume * masterVolume;
                ambientAudioSource.Play();
                Debug.Log("Playing ambient sound: " + clip.name);

                // Чекаємо поки трек завершиться
                yield return new WaitForSeconds(clip.length);

                // Пауза перед наступним треком
                yield return new WaitForSeconds(ambientDelay);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 20f));
            }
        }
    }
}