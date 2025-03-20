using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseShower : MonoBehaviour
{
    [SerializeField] private bool showCursor = true;

    void Awake()
    {
        if (gameObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            Cursor.visible = showCursor;
            Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}