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
    private AudioListener audioListener;

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
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = isMusicOn;
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (audioListener != null)
        {
            audioListener.enabled = isMusicOn;
        }
        else
        {
            Debug.LogError("AudioListener is not assigned.");
        }
    }
}