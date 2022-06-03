using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MovementStartegy
{
    public Transform updateTransform(Transform obstacle, float speed);
}