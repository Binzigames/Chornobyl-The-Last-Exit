using UnityEngine;

public class NotebookHandler : MonoBehaviour
{
    public string notebookText;
    public string notebookTextUA;
    public GameObject interactionPrompt; 

    private bool isPlayerNearby = false;
    private PlayerUIController playerUI;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        playerUI = FindObjectOfType<PlayerUIController>();
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (playerUI != null)
            {
                playerUI.ShowNotebookUI(notebookText , notebookTextUA);
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

