using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
두가지 방법으로 사용될 수 있음
1. 이 스크립트를 붙인 오브젝트를 스테이지에 배치
    - 이 경우 isSpawned를 true로 설정해야됨
    - public으로 선언된 변수들을 설정해 주어야 함 
2. 이 스크립트를 붙인 오브젝트를 ObstacleSpawnManager를 통해 생성
*/
public class Obstacle : MonoBehaviour
{
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
            GameManager.Instance.GameOver(GameOverState.KillAnimal);
        }
    }
}
