using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MovementStartegy
{
    public void Move(float speed, Vector3 spawnPos, Vector3 destPos);
}