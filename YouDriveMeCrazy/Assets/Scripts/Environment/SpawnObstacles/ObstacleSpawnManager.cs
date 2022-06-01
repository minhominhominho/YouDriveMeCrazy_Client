using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleEnum { Empty, Chicken, Doe, Fox, Horse, Man, Bus, BlueCar, Van, Truck }
public enum MovementEnum { Empty, Static, ForwardMoving, StaccatoMoving }
public enum AttributeEnum { Empty, KlaxonInteractable, Jumping, Rotating }

public class ObstacleSpawnManager : MonoBehaviour
{
    public GameObject[] allObstacles;           // All Obstacle objects
    public ObstacleEnum obstacleToBeSpawned;    // The obstacle object to spawn in this area
    public MovementEnum movementStrategy;       // Movement Strategy
    public AttributeEnum[] attributeStrategy;   // Attribute Strategy

    private GameObject obstaclePrefab;
    private StrategyPatternObstacle strategyPatternObstacle;


    public float speed = 5;
    public Transform spawnPos;
    public Transform destPos;
    private bool isSpawned;


    private void Start()
    {
        if (obstacleToBeSpawned == ObstacleEnum.Empty)
        {
            obstacleToBeSpawned = (ObstacleEnum)Random.Range(1, System.Enum.GetValues(typeof(ObstacleEnum)).Length);
        }
        obstaclePrefab = allObstacles[(int)obstacleToBeSpawned - 1];
        strategyPatternObstacle = obstaclePrefab.GetComponent<StrategyPatternObstacle>();

        switch (movementStrategy)
        {
            case MovementEnum.Empty:
                movementStrategy = (MovementEnum)Random.Range(1, System.Enum.GetValues(typeof(MovementEnum)).Length);
                break;
            case MovementEnum.Static:
                //obstaclePrefab.AddComponent <>
                break;
            case MovementEnum.ForwardMoving:
                break;
            case MovementEnum.StaccatoMoving:
                break;
        }

    }

    public void SpawnRandomAnimal()
    {
        // // 재생성 방지
        // if (!isSpawned)
        // {
        //     isSpawned = true;
        //     GameObject g = Instantiate(allObstacles[(int)obstacleToBeSpawned - 1], spawnPos.position, Quaternion.identity);

        //     g.GetComponent<Obstacle>().setObstacle(speed, isKlaxonInteractable, klaxonRequiredTime);
        //     g.transform.LookAt(destPos.position, transform.up);
        // }
    }
}