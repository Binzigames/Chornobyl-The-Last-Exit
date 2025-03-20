using UnityEngine;

public class NotebookHandler : MonoBehaviour
{
    public string notebookText; // ����� ���������
    public GameObject interactionPrompt; // ��'��� ������� "�������� E"

    private bool isPlayerNearby = false;
    private PlayerUIController playerUI;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        playerUI = FindObjectOfType<PlayerUIController>(); // ��������� UI-��������� ������
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

