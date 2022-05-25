using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이거만 붙여도 동작함(미리 생성해두는 애들에 대해서)
public class Obstacle : MonoBehaviour
{
    public float speed = 10f;
    private Transform car;
    public bool isKlaxonInteractable = false;
    public float klaxonDistance = 15;


    void Start()
    {
        // 애니메이션 설정
        car = CarController.carController.transform;
    }

    public void setObstacle(float speed, bool isKlaxonInteractable)
    {
        this.speed = speed;
        this.isKlaxonInteractable = isKlaxonInteractable;
    }

    void Update()
    {
        if(!isKlaxonInteractable){
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else
       {
            if(Vector3.Distance(car.position,this.transform.position) < klaxonDistance){
                if(CarController.carController.isKlaxonPressing){
                    // 애니메이션 설정
                    isKlaxonInteractable = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
