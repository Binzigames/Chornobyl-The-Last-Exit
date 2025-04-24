using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour
{
    public List<Transform> cameraPositions;
    public float fadeDuration = 0.5f;
    public bool useFollowCamera = false;
    public bool returnToFollowCameraOnExit = false;

    private Transform cameraTransform;
    private CameraFollow cameraFollow;
    private Transform playerTransform;
    private CanvasGroup fadeCanvas;
    private Image fadeImage;
    private Stack<(Vector3, Quaternion)> cameraHistory = new Stack<(Vector3, Quaternion)>();
    private bool playerAlreadyEntered = false;
    private bool lastFollowState;

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

        lastFollowState = useFollowCamera;
    }

    void Update()
    {
        if (useFollowCamera != lastFollowState)
        {
            cameraFollow.enabled = useFollowCamera;
            lastFollowState = useFollowCamera;
        }

        if (cameraFollow.enabled && cameraFollow.target == null)
        {
            cameraFollow.target = playerTransform;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (returnToFollowCameraOnExit && playerAlreadyEntered)
        {
            cameraFollow.enabled = true;
            cameraFollow.target = playerTransform;
        }
        else
        {
            cameraFollow.enabled = false;
        }

        if (playerAlreadyEntered && cameraHistory.Count > 0)
        {

            var previousCamera = cameraHistory.Pop();
            SwitchCameraPosition(previousCamera.Item1, previousCamera.Item2);

            playerAlreadyEntered = false;
        }
        else
        {
            cameraHistory.Push((cameraTransform.position, cameraTransform.rotation));

            if (useFollowCamera)
            {
                cameraFollow.enabled = true;
                cameraFollow.target = playerTransform;
            }
            else
            {
                Transform nextCamPos = GetClosestCameraPosition();
                if (nextCamPos != null && nextCamPos.position != cameraTransform.position)
                {
                    cameraFollow.enabled = false;
                    cameraFollow.target = null;
                    SwitchCameraPosition(nextCamPos.position, nextCamPos.rotation);
                }
            }

            playerAlreadyEntered = true;
        }
    }

    private void SwitchCameraPosition(Vector3 newCameraPos, Quaternion newCameraRot)
    {
        StopAllCoroutines(); // На всяк випадок
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
