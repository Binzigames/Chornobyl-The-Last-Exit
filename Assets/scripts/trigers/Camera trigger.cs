using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour
{
    public List<Transform> cameraPositions;
    public float fadeDuration = 0.5f;

    private Transform cameraTransform;
    private Transform playerTransform;
    private CanvasGroup fadeCanvas;
    private Image fadeImage;
    private Stack<(Vector3, Quaternion)> cameraHistory = new Stack<(Vector3, Quaternion)>();

    void Start()
    {
        cameraTransform = Camera.main.transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject fadeObj = GameObject.Find("FadeCanvas");
        if (fadeObj != null)
        {
            fadeCanvas = fadeObj.GetComponent<CanvasGroup>();
        }
        else
        {
            fadeObj = new GameObject("FadeCanvas");
            Canvas canvas = fadeObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            fadeCanvas = fadeObj.AddComponent<CanvasGroup>();
            fadeImage = fadeObj.AddComponent<Image>();
            fadeImage.color = Color.black;

            fadeCanvas.alpha = 0;
            fadeCanvas.blocksRaycasts = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cameraHistory.Count > 0)
            {
                (Vector3 previousPosition, Quaternion previousRotation) = cameraHistory.Pop();
                SwitchCameraPosition(previousPosition, previousRotation);
            }
            else
            {
                Transform closestCameraPosition = GetClosestCameraPosition();
                if (closestCameraPosition != null && closestCameraPosition.position != cameraTransform.position)
                {
                    cameraHistory.Push((cameraTransform.position, cameraTransform.rotation));
                    SwitchCameraPosition(closestCameraPosition.position, closestCameraPosition.rotation);
                }
            }
        }
    }

    private void SwitchCameraPosition(Vector3 newCameraPos, Quaternion newCameraRot)
    {
        StartCoroutine(FadeToNewPosition(newCameraPos, newCameraRot));
    }

    private IEnumerator FadeToNewPosition(Vector3 newCameraPos, Quaternion newCameraRot)
    {
        float time = 0f;
        fadeCanvas.alpha = 0;
        fadeCanvas.alpha = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            yield return null;
        }

        cameraTransform.position = newCameraPos;
        cameraTransform.rotation = newCameraRot;

        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            yield return null;
        }
    }

    private Transform GetClosestCameraPosition()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (Transform camPos in cameraPositions)
        {
            float distance = Vector3.Distance(playerTransform.position, camPos.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = camPos;
            }
        }
        return closest;
    }
}
