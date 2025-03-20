using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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