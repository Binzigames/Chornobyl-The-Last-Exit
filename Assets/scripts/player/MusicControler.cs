using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicControler : MonoBehaviour
{
    [Header("UI")]
    public static MusicControler instance;
    public Button toggleMusicButton;

    [Header("BOOLING")]
    public bool isMusicOn = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load saved music state
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        ApplyVolume();
    }

    void Start()
    {
        if (toggleMusicButton != null)
        {
            toggleMusicButton.onClick.AddListener(ToggleMusic);
        }
        else
        {
            Debug.LogWarning("ToggleMusicButton is not assigned in the inspector.");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyVolume();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        ApplyVolume();

        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyVolume()
    {
        AudioListener.volume = isMusicOn ? 1f : 0f;
    }
}
