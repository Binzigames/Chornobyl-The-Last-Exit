using TMPro;
using UnityEngine;

public class LocalizationHandlerString : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TextToLocalized;
    [SerializeField]
    private string textToLoad;
    private string EnglishText;

    private LocalizationManager localizationManager;

    private void Start()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();


        EnglishText = TextToLocalized.text;
    }

    void Update()
    {
        if (localizationManager != null)
        {

            TextToLocalized.text = localizationManager.IsUkranian ? textToLoad : EnglishText;
        }
    }
}

