using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyPatternObstacle : MonoBehaviour
{
    private MovementStartegy movementStartegy;
    private List<AttributeStartegy> attributeStartegies;


    public bool isSpawned;
    public float speed = 10f;
    private Transform car;



    void Start()
    {
        // 애니메이션 설정
        car = CarController.carController.transform;
    }

    public void setObstacle(float speed, bool isKlaxonInteractable, float time)
    {
        this.speed = speed;

     

        isSpawned = true;
    }

    void Update()
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            GameManager.Instance.GameOver(GameManager.GameState.KillAnimal);
        }
    }
}
