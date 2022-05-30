using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiperPressingNeededZone : MonoBehaviour
{
    public int requiredWiperPressing;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSpeaker;
    [SerializeField] private AudioClip startEngineSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarController.carController.coverWindow(requiredWiperPressing);
        }
    }
}
