using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovementStrategy : MovementStartegy
{
    public Transform updateTransform(Transform obstacle, float speed)
    {
        Transform newTransform = obstacle;
        Vector3 eulerRotation = newTransform.rotation.eulerAngles;
        newTransform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
        newTransform.position += newTransform.forward * speed * Time.deltaTime;
        return newTransform;
    }
}
