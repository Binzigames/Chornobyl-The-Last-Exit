using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void restart_with_corutine()
    {
        StartCoroutine(restartCoroutine(SceneManager.GetActiveScene().buildIndex , 6));
    }
    private IEnumerator restartCoroutine(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(sceneIndex);
    }

    // Функція для зміни сцени після очікування завершення попередньої дії
    public void ChangeSceneWithDelay(int sceneIndex)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneIndex, 6));
    }

    private IEnumerator ChangeSceneCoroutine(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(sceneIndex);
    }
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
