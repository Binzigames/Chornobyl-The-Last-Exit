using UnityEngine;


public class RadiationZone : MonoBehaviour
{
    public float radiationIncreaseRate = 1f; // �������� ��������� �������
    public float maxRadiation = 100f; // ����������� �������� �������
    private bool isInRadiationZone = false; // �� ����������� ������� � ��� �������

    private void OnTriggerEnter(Collider other)
    {
        // ��������, �� �� �������
        if (other.CompareTag("Player"))
        {
            isInRadiationZone = true; // ������� �������� � ���� �������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ��������, �� �� �������
        if (other.CompareTag("Player"))
        {
            isInRadiationZone = false; // ������� ������� ���� �������
        }
    }

    public bool IsInRadiationZone()
    {
        return isInRadiationZone;
    }
}

