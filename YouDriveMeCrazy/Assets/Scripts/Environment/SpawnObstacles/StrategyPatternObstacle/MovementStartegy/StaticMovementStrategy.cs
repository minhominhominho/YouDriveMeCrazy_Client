using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMovementStrategy : MovementStartegy
{
    public Transform updateTransform(Transform obstacle, float speed)
    {
        return obstacle;
    }
}
