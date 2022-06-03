using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingAttributeStartegy : AttributeStartegy
{
    private bool firstRotating = true;
    private float timer = 2;
    private float rotateDegree = 60;
    private const float rotatingCycle = 5;

    public Transform Excute(Transform obstacle)
    {
        Transform newTransform = obstacle;
        timer += Time.deltaTime;

        if (firstRotating)
        {
            firstRotating = false;
            newTransform.Rotate(newTransform.up * rotateDegree / 2);
        }

        if (timer >= rotatingCycle)
        {
            timer = 0;
            rotateDegree *= -1;
            newTransform.Rotate(newTransform.up * rotateDegree);
        }

        return obstacle;
    }
}