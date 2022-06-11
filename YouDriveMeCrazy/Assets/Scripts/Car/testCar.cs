using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCar : MonoBehaviour
{
private void OnCollisionEnter(Collision other) {
    print(other.collider.name);
}
}
