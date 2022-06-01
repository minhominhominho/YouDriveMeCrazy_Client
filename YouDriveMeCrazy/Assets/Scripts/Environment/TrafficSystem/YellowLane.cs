using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowLane : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Car"){
            GameManager.Instance.GameOver(GameManager.GameState.MidLaneCross);
            this.GetComponent<Collider>().enabled = false;
        }

    }

}
