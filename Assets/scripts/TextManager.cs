using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        [TextArea(3, 10)] public string textEN;
        [TextArea(3, 10)] public string textUA;
        public float displayTime = 2f;
        public bool hasSprite = false;
        public Sprite sprite;
        public SpriteEffect spriteEffect = SpriteEffect.None;
    }

    public enum SpriteEffect
    {
        None,
        FadeIn,
        ScaleUp,
        Disappear
    }

    public List<Slide> slides;
    public TextMeshProUGUI textDisplay;
    public float typingSpeed = 0.05f;
    public CanvasGroup canvasGroup;
    public int nextSceneIndex = 1;
    public Image spriteDisplay;

    private int currentSlideIndex = 0;
    private bool isTyping = false;
    private bool skipSlide = false;
    private bool isUkrainian = false;
    private LocalizationManager localizationManager;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (textDisplay == null || canvasGroup == null || spriteDisplay == null || slides == null || slides.Count == 0)
        {
            Debug.LogError("IntroManager: ќдин або к≥лька об'Їкт≥в не призначен≥ або список слайд≥в порожн≥й!");
            return;
        }

        localizationManager = FindObjectOfType<LocalizationManager>();
        if (localizationManager != null)
        {
            isUkrainian = localizationManager.IsUkranian;
        }
        else
        {
            isUkrainian = false;
        }

        currentCoroutine = StartCoroutine(ShowSlides());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skipSlide = true;
        }
    }

    IEnumerator ShowSlides()
    {
        foreach (var slide in slides)
        {
            skipSlide = false;

            string currentText = isUkrainian ? slide.textUA : slide.textEN;

            if (slide.hasSprite && slide.sprite != null)
            {
                spriteDisplay.sprite = slide.sprite;
                spriteDisplay.SetNativeSize();
                spriteDisplay.enabled = true;
                spriteDisplay.color = new Color(1, 1, 1, 1);
                spriteDisplay.transform.localScale = Vector3.one;

                yield return StartCoroutine(ApplySpriteEffect(slide.spriteEffect));
            }
            else
            {
                spriteDisplay.sprite = null;
                spriteDisplay.enabled = false;
            }

            yield return StartCoroutine(TypeText(currentText));

            float elapsedTime = 0f;
            while (elapsedTime < slide.displayTime && !skipSlide)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return StartCoroutine(FadeOut());
            textDisplay.text = "";
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in text)
        {
            if (skipSlide)
            {
                textDisplay.text = text;
                break;
            }
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(0.5f);
        canvasGroup.alpha = 1;
    }

    IEnumerator ApplySpriteEffect(SpriteEffect effect)
    {
        if (spriteDisplay == null) yield break;

        if (effect == SpriteEffect.FadeIn)
        {
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                spriteDisplay.color = new Color(1, 1, 1, Mathf.Clamp01(alpha));
                yield return null;
            }
        }
        else if (effect == SpriteEffect.ScaleUp)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime;
                spriteDisplay.transform.localScale = Vector3.Lerp(startScale, endScale, time);
                yield return null;
            }
        }
        else if (effect == SpriteEffect.Disappear)
        {
            yield return new WaitForSeconds(1f);
            spriteDisplay.color = new Color(1, 1, 1, 0);
        }
    }

    public void SwitchToUkrainian()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        isUkrainian = true;
        currentCoroutine = StartCoroutine(ShowSlides());
    }

    public void SwitchToEnglish()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        isUkrainian = false;
        currentCoroutine = StartCoroutine(ShowSlides());
    }
}
