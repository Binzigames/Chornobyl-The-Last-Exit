using UnityEngine;


public class RadiationZone : MonoBehaviour
{
    public float radiationIncreaseRate = 1f; // Швидкість збільшення радіації
    public float maxRadiation = 100f; // Максимальне значення радіації
    private bool isInRadiationZone = false; // Чи знаходиться гравець у зоні радіації

    private void OnTriggerEnter(Collider other)
    {
        // Перевірка, чи це гравець
        if (other.CompareTag("Player"))
        {
            isInRadiationZone = true; // Гравець потрапив у зону радіації
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Перевірка, чи це гравець
        if (other.CompareTag("Player"))
        {
            isInRadiationZone = false; // Гравець покинув зону радіації
        }
    }

    public bool IsInRadiationZone()
    {
        return isInRadiationZone;
    }
}

