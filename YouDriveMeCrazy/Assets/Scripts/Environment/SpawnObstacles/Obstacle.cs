using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Obstacles { Empty, Chicken, Doe, Fox, Horse, Man, Bus, BlueCar, Van, Truck }
public enum ObstacleType { Idle, Walk }

public class Obstacle : MonoBehaviour
{
    // All Obstacle objects
    public GameObject[] allObstacles;

    // The obstacle object to spawn in this area
    public Obstacles obstacleToBeSpawned;
    public ObstacleType obstacleType;

    public float timer = 15;
    public float speed = 5;
    public Transform spawnPos;
    public Transform destPos;
    private bool isSpawned;

    private void Start()
    {
        if (obstacleToBeSpawned == Obstacles.Empty)
        {
            obstacleToBeSpawned = (Obstacles)Random.Range(1, System.Enum.GetValues(typeof(Obstacles)).Length);
        }
    }

    public void SpawnRandomAnimal()
    {
        // 재생성 방지
        if (!isSpawned)
        {
            isSpawned = true;
            GameObject g = Instantiate(allObstacles[(int)obstacleToBeSpawned - 1], spawnPos.position, Quaternion.identity);
            g.GetComponent<ObstacleMovementController>().setObstacle(timer, speed);
            g.transform.LookAt(destPos.position, transform.up);
        }
    }
}
