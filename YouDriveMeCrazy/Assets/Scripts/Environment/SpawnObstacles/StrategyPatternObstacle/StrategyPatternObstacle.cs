using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyPatternObstacle : MonoBehaviour
{
    private MovementStartegy movementStartegy;
    private List<AttributeStartegy> attributeStartegies;


    public bool isSpawned;

    private float speed = 10f;
    private Vector3 spawnPos;
    private Vector3 destPos;
    

    public void setObstacle(float speed, bool isKlaxonInteractable, float time)
    {
        this.speed = speed;
        isSpawned = true;
    }

    void Update()
    {
        if (isSpawned)
        {
            movementStartegy.Move(speed,spawnPos,destPos);
            foreach (AttributeStartegy a in attributeStartegies)
            {
                a.Excute();
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
