using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaccatoMovementStartegy : MovementStartegy
{
    private float timer;
    private const float staccatoCycle = 5;


    public Transform updateTransform(Transform obstacle, float speed)
    {
        Transform newTransform = obstacle;
        timer += Time.deltaTime;

        if (timer >= staccatoCycle)
        {
            timer = 0;
        }

        if (timer < staccatoCycle / 2)
        {
            Vector3 eulerRotation = newTransform.rotation.eulerAngles;
            newTransform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
            newTransform.position += newTransform.forward * speed * Time.deltaTime;
        }

        return newTransform;
    }
}
