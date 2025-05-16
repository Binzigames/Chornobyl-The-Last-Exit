using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
    public Transform cameraPoint;            
    public float fadeDuration = 0.5f;
    public bool switchToFollowOnExit = true;

    private Transform cameraTransform;
    private CameraFollow cameraFollow;
    private Transform playerTransform;
    private CanvasGroup fadeCanvas;
    private Image fadeImage;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraFollow = cameraTransform.GetComponent<CameraFollow>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        GameObject fadeObj = GameObject.Find("FadeCanvas");
        if (fadeObj != null)
        {
            fadeCanvas = fadeObj.GetComponent<CanvasGroup>();
            fadeImage = fadeObj.GetComponent<Image>();
        }
        else
        {
            fadeObj = new GameObject("FadeCanvas");
            Canvas canvas = fadeObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            fadeCanvas = fadeObj.AddComponent<CanvasGroup>();
            fadeImage = fadeObj.AddComponent<Image>();
            fadeImage.color = Color.black;
            fadeImage.raycastTarget = false;

            fadeCanvas.alpha = 0;
            fadeCanvas.blocksRaycasts = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (cameraPoint != null)
        {
            cameraFollow.enabled = false;
            cameraFollow.target = null;

            SwitchCameraPosition(cameraPoint.position, cameraPoint.rotation);
        }
        else
        {
            Debug.LogWarning("CameraTrigger: Не вказано cameraPoint!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (switchToFollowOnExit)
        {
            cameraFollow.enabled = true;
            cameraFollow.target = playerTransform;
        }
    }

    private void SwitchCameraPosition(Vector3 newCameraPos, Quaternion newCameraRot)
    {
        StopAllCoroutines();
        StartCoroutine(FadeToNewPosition(newCameraPos, newCameraRot));
    }

    private IEnumerator FadeToNewPosition(Vector3 newCameraPos, Quaternion newCameraRot)
    {
        float time = 0f;

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

        fadeCanvas.alpha = 0f;
    }
}
