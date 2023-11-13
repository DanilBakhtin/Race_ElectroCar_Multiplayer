using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private UI_Controller controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other)
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BatteryController>().goToCheckPoint();
        }
    }
}
