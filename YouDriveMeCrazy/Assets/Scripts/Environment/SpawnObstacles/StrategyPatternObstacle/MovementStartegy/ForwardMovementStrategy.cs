using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovementStrategy : MonoBehaviour, MovementStartegy
{
    public void Move(float speed, Vector3 spawnPos, Vector3 destPos)
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
