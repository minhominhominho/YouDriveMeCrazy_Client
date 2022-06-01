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

    [Header("Klaxon")]
    public bool isKlaxonInteractable = false;
    public float klaxonRequiredTime = 10;

    public float klaxonDistance = 15;
    private float klaxonPressingTime;


    void Start()
    {
        // 애니메이션 설정
        car = CarController.carController.transform;
    }

    public void setObstacle(float speed, bool isKlaxonInteractable, float time)
    {
        this.speed = speed;

        if (isKlaxonInteractable)
        {
            this.isKlaxonInteractable = isKlaxonInteractable;
            klaxonRequiredTime = time;
        }

        isSpawned = true;
    }

    void Update()
    {
        if (isSpawned)
        {
            if (!isKlaxonInteractable)
            {
                transform.position += transform.forward * Time.deltaTime * speed;
            }
            else
            {
                if (Vector3.Distance(car.position, this.transform.position) < klaxonDistance)
                {
                    // 애니메이션 설정
                    if (CarController.carController.isKlaxonPressing)
                    {
                        klaxonPressingTime += Time.deltaTime;

                        if (klaxonPressingTime > klaxonRequiredTime)
                        {
                            isKlaxonInteractable = false;
                        }
                    }
                    else
                    {
                        klaxonPressingTime = 0;
                    }
                }
            }

            // 차랑 너무 멀어지면 그냥 없애버리기
            if (Vector3.Distance(car.position, this.transform.position) > 100)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            GameManager.Instance.GameOver(GameManager.GameState.KillAnimal);
        }
    }
}
