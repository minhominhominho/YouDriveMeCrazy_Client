using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTriggerZone : MonoBehaviour
{
    private StrategyPatternObstacleSpawnManager strategyPatternObstacleSpawnManager;

    private void Start()
    {
        strategyPatternObstacleSpawnManager = this.GetComponentInParent<StrategyPatternObstacleSpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            strategyPatternObstacleSpawnManager.ActivateObstacleInGame();
        }
    }
}
