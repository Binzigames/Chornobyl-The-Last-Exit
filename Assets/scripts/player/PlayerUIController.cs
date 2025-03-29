using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [Header("Radiation")]
    public float currentRadiation = 0f;
    public float maxRadiation = 100f;
    public Slider radiationSlider;
    private CanvasGroup sliderCanvasGroup;
    private RadiationZone[] radiationZones;

    [Header("Pause menu")]
    public bool InPause = false;
    public GameObject PauseMenu = null;
    private CanvasGroup pauseCanvasGroup;

    [Header("Death UI")]
    public GameObject GamePanel = null;
    public GameObject Screamer = null;
    public TextMeshProUGUI deathMessage;
    public float screamerDuration = 3f;
    public AudioSource screamerAudio; // Додано змінну для звуку скрімера

    [Header("Notebook UI")]
    public GameObject notebookUI;
    public TextMeshProUGUI notebookTextUI;
    private bool isNotebookOpen = false;

    private void Start()
    {
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isNotebookOpen)
            {
                CloseNotebookUI();
            }
            else if (!InPause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (radiationSlider == null || sliderCanvasGroup == null || radiationZones.Length == 0)
        {
            return;
        }

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
            {
                StartCoroutine(FadeInSlider());
            }

            radiationSlider.value = currentRadiation / maxRadiation;
            currentRadiation = Mathf.Min(currentRadiation, maxRadiation);

            if (currentRadiation >= maxRadiation)
            {
                Die("NOW YOU ARE GLOWING LIKE A TORCH!");
            }
        }
        else
        {
            if (sliderCanvasGroup.alpha > 0f)
            {
                StartCoroutine(FadeOutSlider());
            }
        }
    }

    public void Die(string causeOfDeath)
    {
        GamePanel.SetActive(false);
        Screamer.SetActive(true);
        if (screamerAudio != null)
        {
            screamerAudio.Play(); // Відтворення звуку
        }
        if (deathMessage != null)
        {
            deathMessage.text = causeOfDeath;
        }
        StartCoroutine(HandleDeathScreen());
    }

    private IEnumerator HandleDeathScreen()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        Screamer.SetActive(false);
        GamePanel.SetActive(true);
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

    public void ShowNotebookUI(string text)
    {
        notebookUI.SetActive(true);
        notebookTextUI.text = text;
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
}