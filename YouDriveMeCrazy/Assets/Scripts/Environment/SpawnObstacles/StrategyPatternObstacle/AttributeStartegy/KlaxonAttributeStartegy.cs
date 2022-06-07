using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonAttributeStartegy : AttributeStartegy
{
    private Transform car;

    private const float klaxonDistance = 15;
    private const float klaxonRequiredTime = 3;
    private const float runsAwaySpeed = 5;

    private float klaxonPressingTime;
    private bool isInteracted;



    void Start()
    {
        car = CarController.carController.transform;
    }

    public Transform Excute(Transform obstacle)
    {
        Transform newTransform = obstacle;

        if (!isInteracted)
        {
            if (Vector3.Distance(CarController.carController.transform.position, obstacle.position) < klaxonDistance)
            {
                if (CarController.carController.isKlaxonPressing)
                {
                    klaxonPressingTime += Time.deltaTime;

                    if (klaxonPressingTime > klaxonRequiredTime)
                    {
                        isInteracted = true;
                        try
                        {
                            obstacle.GetComponent<Animator>().SetBool("isRun", true);
                        }
                        catch { }
                        CarController.carController.KlaxonCount_Accievement += 1;
                    }
                }
                else
                {
                    klaxonPressingTime = 0;
                }
            }
        }
        else
        {
            Vector3 eulerRotation = newTransform.rotation.eulerAngles;
            newTransform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
            newTransform.position += newTransform.forward * runsAwaySpeed * Time.deltaTime;
        }

        return newTransform;
    }
}