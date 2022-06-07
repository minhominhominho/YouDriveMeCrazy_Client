using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{

    [Header("Traffic Light")]
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject yellowLight;
    [SerializeField] private GameObject greenLight;


    [Header("Cycle Time")]
    [SerializeField] private float redLightTime = 10f;
    [SerializeField] private float yellowLightTime = 5f;
    [SerializeField] private float greenLightTime = 20f;

    private BoxCollider sensingArea;

    public bool isRandom;
    private bool isRedLight = false;
    private bool isWorking = true;

    private float timer;

    private const float updatingTime = 0.01f;


    void Start()
    {
        sensingArea = GetComponent<BoxCollider>();
        sensingArea.enabled = false;

        redLight.SetActive(true);
        yellowLight.SetActive(false);
        greenLight.SetActive(false);

        isRedLight = true;
        sensingArea.enabled = true;

        if (isRandom)
        {
            StartCoroutine(Timer());
        }
    }


    // by 상민, If car cross the reference line on a red light, colliding is detected    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && isRedLight)
        {
            isWorking = false;
            GameManager.Instance.GameOver(GameManager.GameState.TrafficLightViolation);
        }
    }

    public void TriggerTrafficLight()
    {
        StartCoroutine(Timer());
    }

    // by 상민, red,yellow,greenLightTime에 따라 신호등 동작, green->yellow->red 순으로 바뀜 
    IEnumerator Timer()
    {
        timer = 0f;

        while (isWorking)
        {
            timer += updatingTime;

            if (timer >= redLightTime)
            {
                redLight.SetActive(false);
                greenLight.SetActive(true);
                yellowLight.SetActive(false);

                isRedLight = false;
                sensingArea.enabled = false;
            }
            else if (timer >= redLightTime + greenLightTime)
            {
                redLight.SetActive(value: false);
                greenLight.SetActive(false);
                yellowLight.SetActive(true);


            }
            else if (timer >= redLightTime + greenLightTime + yellowLightTime)
            {
                redLight.SetActive(true);
                greenLight.SetActive(false);
                yellowLight.SetActive(false);

                isRedLight = true;
                sensingArea.enabled = true;
                timer = 0;
            }
            yield return new WaitForSeconds(updatingTime);
        }
        yield return null;
    }
}
