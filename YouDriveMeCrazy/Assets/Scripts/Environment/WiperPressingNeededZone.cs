using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiperPressingNeededZone : MonoBehaviour
{
    public int requiredWiperPressing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarController.carController.coverWindow(requiredWiperPressing);
        }
    }
}
