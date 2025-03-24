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

    // ������� ��� ���� ����� ���� ���������� ���������� ���������� 䳿
    public void ChangeSceneWithDelay(int sceneIndex)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneIndex, 6));
    }

    private IEnumerator ChangeSceneCoroutine(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(sceneIndex);
    }
    // ������� ��� ���� ����� �� �������
    public void ChangeScene(int sceneIndex)
    {
        // ��������, �� ����� � ����� �������� ����
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("����� � ����� �������� �� ����!");
        }
    }

    // ������� ��� ������ � ���
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("��� �������!");
    }

    // ������� ��� ��������� ��� ����������� ��'����
    public void ToggleObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(!obj.activeSelf); // ����� ��������� ��'����
        }
        else
        {
            Debug.LogError("��'��� �� ���������!");
        }
    }

    // ������� ��� �������� URL � �������
    public void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
            Debug.Log("³������ URL: " + url);
        }
        else
        {
            Debug.LogError("URL �� ���� ���� �������!");
        }
    }


}
