using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiationdecres : MonoBehaviour
{
    public float radiationDecreaseRate = 10f;
    private PlayerUIController playerUI;

    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUIController>();
        if (playerUI == null || playerUI.radiationSlider == null)
        {
            Debug.LogError("PlayerUIController ��� RadiationSlider �� �������� � ����.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerUI != null)
        {
            playerUI.currentRadiation -= radiationDecreaseRate * Time.deltaTime;
            playerUI.currentRadiation = Mathf.Max(playerUI.currentRadiation, 0f);
            playerUI.radiationSlider.value = playerUI.currentRadiation / playerUI.maxRadiation;
            playerUI.SetRadiationSliderVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerUI != null)
        {
            playerUI.SetRadiationSliderVisibility(false);
        }
    }
}
