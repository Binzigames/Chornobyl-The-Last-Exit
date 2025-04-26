using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public GameObject leverTextUI;
    public GameObject[] objectsToToggle;
    public AudioSource audioSource;
    public GameObject Lever;
    public int Rot = 0;
    public float up = 0;

    public bool HaveALever = false;
    private bool isPlayerNear = false;
    private bool isActivated = false;

    private Vector3 originalLeverLocalPosition;
    private Quaternion originalLeverLocalRotation;

    void Start()
    {
        leverTextUI.SetActive(false);

        if (HaveALever && Lever != null)
        {
            originalLeverLocalPosition = Lever.transform.localPosition;
            originalLeverLocalRotation = Lever.transform.localRotation;
        }
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

        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }

        if (HaveALever && Lever != null)
        {
            if (isActivated)
            {
                Lever.transform.localRotation *= Quaternion.Euler(Rot, 0f, 0f);
                Lever.transform.localPosition = new Vector3(-0.1875f, up, 0.125f);
            }
            else
            {
                Lever.transform.localPosition = originalLeverLocalPosition;
                Lever.transform.localRotation = originalLeverLocalRotation;
            }
        }

        if (audioSource != null)
        {
            audioSource.Play();
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
