using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrafficZone : MonoBehaviour
{
    private TrafficLightController trafficLightController;

    private void Start()
    {
        trafficLightController = this.GetComponentInParent<TrafficLightController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            trafficLightController.TriggerTrafficLight();
        }
    }
}
