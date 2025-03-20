using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelDescriptionText;
    public CanvasGroup canvasGroup;
    public float displayTime = 3f;
    public float fadeDuration = 1f;

    [Header("Text")]
    public string levelName;
    public string levelDescription;

    void Start()
    {
        ShowLevelInfo(levelName, levelDescription);
    }

    public void ShowLevelInfo(string levelName, string levelDescription)
    {
        levelNameText.text = levelName;
        levelDescriptionText.text = levelDescription;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        uiPanel.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(displayTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        uiPanel.SetActive(false);
    }
}
