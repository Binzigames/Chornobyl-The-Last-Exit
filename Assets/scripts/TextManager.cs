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
        [TextArea(3, 10)] public string text; // Текст слайду
        public float displayTime = 2f; // Час показу слайду
        public bool hasSprite = false; // Чи є спрайт на слайді
        public Sprite sprite; // Спрайт для відображення
        public SpriteEffect spriteEffect = SpriteEffect.None; // Ефект для спрайту
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
    public float typingSpeed = 0.05f; // Швидкість друку тексту
    public CanvasGroup canvasGroup;
    public int nextSceneIndex = 1; // Індекс сцени для переходу
    public Image spriteDisplay; // Поле для спрайта

    private int currentSlideIndex = 0;
    private bool isTyping = false;
    private bool skipSlide = false;

    void Start()
    {
        if (textDisplay == null || canvasGroup == null || spriteDisplay == null || slides == null || slides.Count == 0)
        {
            Debug.LogError("IntroManager: Один або кілька об'єктів не призначені або список слайдів порожній!");
            return;
        }
        StartCoroutine(ShowSlides());
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
            if (slide.hasSprite && slide.sprite != null && spriteDisplay != null)
            {
                spriteDisplay.sprite = slide.sprite;
                spriteDisplay.SetNativeSize();
                spriteDisplay.enabled = true;
                spriteDisplay.color = new Color(1, 1, 1, 1);
                spriteDisplay.transform.localScale = Vector3.one;
                yield return StartCoroutine(ApplySpriteEffect(slide.spriteEffect));
                spriteDisplay.enabled = false;
            }
            else if (spriteDisplay != null)
            {
                spriteDisplay.sprite = null;
                spriteDisplay.color = new Color(1, 1, 1, 0);
                spriteDisplay.enabled = false;
            }

            yield return StartCoroutine(TypeText(slide.text));
            float elapsedTime = 0f;
            while (elapsedTime < slide.displayTime && !skipSlide)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return StartCoroutine(FadeOut());
            textDisplay.text = "";
        }
        // Інтро завершене, переходимо на іншу сцену
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator TypeText(string text)
    {
        if (textDisplay == null) yield break;
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
        if (canvasGroup == null) yield break;
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
                spriteDisplay.color = new Color(1, 1, 1, alpha);
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
}
