using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BatteryController : MonoBehaviour
{
    [SerializeField] private Image batteryIcon;
    [SerializeField] private Image actionIcon;
    [SerializeField] private Text energyText;
    [SerializeField] private Text costEnergyText;

    public void updateEnergyText(float currentEnergy)
    {
        energyText.text = Mathf.RoundToInt(currentEnergy).ToString();
    }

    public void actionOn()
    {
        actionIcon.enabled = true;
        costEnergyText.enabled = true;
    }

    public void actionOff()
    {
        actionIcon.enabled = false;
        costEnergyText.enabled = false;
    }

    public void updateIconBattery(float currentEnergy, float maxEnergy)
    {
        batteryIcon.fillAmount = currentEnergy / maxEnergy;
    }

    public void setCostTrapText(float cost)
    {
        costEnergyText.text = cost.ToString();
    }
}
