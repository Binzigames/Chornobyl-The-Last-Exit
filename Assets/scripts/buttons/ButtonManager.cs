using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Функція для зміни сцени по індексу
    public void ChangeScene(int sceneIndex)
    {
        // Перевірка, чи сцена з таким індексом існує
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Сцена з таким індексом не існує!");
        }
    }

    // Функція для виходу з гри
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Гра закрита!");
    }

    // Функція для активації або деактивації об'єкта
    public void ToggleObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(!obj.activeSelf); // Змінює активність об'єкта
        }
        else
        {
            Debug.LogError("Об'єкт не знайдений!");
        }
    }

    // Функція для відкриття URL у браузері
    public void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
            Debug.Log("Відкрито URL: " + url);
        }
        else
        {
            Debug.LogError("URL не може бути порожнім!");
        }
    }
}