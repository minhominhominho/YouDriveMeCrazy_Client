using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiperPressingNeededZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarController.carController.coverWindow();
        }
    }
}
