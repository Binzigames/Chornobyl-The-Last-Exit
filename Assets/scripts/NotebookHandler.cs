using UnityEngine;

public class NotebookHandler : MonoBehaviour
{
    public string notebookText; // Текст записника
    public GameObject interactionPrompt; // Об'єкт підказки "Натисніть E"

    private bool isPlayerNearby = false;
    private PlayerUIController playerUI;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        playerUI = FindObjectOfType<PlayerUIController>(); // Знаходимо UI-контролер гравця
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (playerUI != null)
            {
                playerUI.ShowNotebookUI(notebookText);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionPrompt.SetActive(false);
        }
    }
}

