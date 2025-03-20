using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public GameObject leverTextUI;
    public GameObject[] objectsToToggle;

    private bool isPlayerNear = false;
    private bool isActivated = false;

    void Start()
    {
        leverTextUI.SetActive(false); 
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E)) 
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        isActivated = !isActivated;

        // Перемикання стану кожного об'єкта
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            leverTextUI.SetActive(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            leverTextUI.SetActive(false);

        }
    }
}
