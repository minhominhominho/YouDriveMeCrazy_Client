using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ObstacleSpawnManager : MonoBehaviour
{
    public List<GameObject> allObstacles;           // All Obstacle objects
    //public ObstacleEnum obstacleToBeSpawned;    // The obstacle object to spawn in this area
    public MovementEnum movementStrategy;       // Movement Strategy
    public AttributeEnum[] attributeStrategy;   // Attribute Strategy

    public float speed = 5;
    public Transform spawnPos;
    public Transform destPos;

    private GameObject obstaclePrefab;
    private StrategyPatternObstacle strategyPatternObstacle;
    private bool isSpawned;


    private void Start()
    {
        Object[] data = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/GamePlay/SpawnObstacles/Objects");

        foreach (Object o in data)
        {
            allObstacles.Add((GameObject)o);
        }


        // if (obstacleToBeSpawned == ObstacleEnum.Empty)
        // {
        //     obstacleToBeSpawned = (ObstacleEnum)Random.Range(1, System.Enum.GetValues(typeof(ObstacleEnum)).Length);
        // }
        // obstaclePrefab = allObstacles[(int)obstacleToBeSpawned - 1];
        // strategyPatternObstacle = obstaclePrefab.GetComponent<StrategyPatternObstacle>();

        // switch (movementStrategy)
        // {
        //     case MovementEnum.Empty:
        //         movementStrategy = (MovementEnum)Random.Range(1, System.Enum.GetValues(typeof(MovementEnum)).Length);
        //         break;
        //     case MovementEnum.Static:
        //         //obstaclePrefab.AddComponent <>
        //         break;
        //     case MovementEnum.ForwardMoving:
        //         break;
        //     case MovementEnum.StaccatoMoving:
        //         break;
        // }

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