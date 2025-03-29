using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RetroButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public string hoverText = "";
    public string defaultText = "";
    private Color defaultColor;
    public Color blinkColor = Color.yellow;

    private void Awake()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();


        buttonText.text = defaultText;
        defaultColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = hoverText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = defaultText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(BlinkEffect());
    }

    private System.Collections.IEnumerator BlinkEffect()
    {
        buttonText.color = blinkColor;
        yield return new WaitForSeconds(0.1f);
        buttonText.color = defaultColor;
    }
}