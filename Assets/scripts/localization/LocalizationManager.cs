using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [Header("translation")]
    public bool IsUkranian = false;
    public Button toggleMusicButton;
    private static LocalizationManager instance;

    private void Awake()
    {
        if (toggleMusicButton != null)
        {
            toggleMusicButton.onClick.AddListener(ToggleLanguage);
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleLanguage()
    {
        IsUkranian = !IsUkranian;
    }
}
