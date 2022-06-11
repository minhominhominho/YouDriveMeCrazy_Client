using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum MovementEnum { Static, ForwardMoving, StaccatoMoving }
public enum AttributeEnum { KlaxonInteractable, Jumping, Rotating }

public class StrategyPatternObstacleSpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;          // The obstacle object to spawn in this area
    public MovementEnum movementStrategy;       // Movement Strategy
    public List<AttributeEnum> attributeStrategies;   // Attribute Strategy

    public bool isPreCreatedObstacle;
    public float speed;
    public Transform spawnPos;
    public Transform destPos;

    private StrategyPatternObstacle strategyPatternObstacle;
    private bool isActivate;


    private void Awake()
    {
        SetObstacle();
    }

    private void SetObstacle()
    {
        // 장애물 생성, parent를 StrategyPatternObstacleSpawnManager로 지정
        GameObject obstacle;
        obstacle = Instantiate(obstaclePrefab, spawnPos.position, Quaternion.identity);
        obstacle.transform.SetParent(gameObject.transform);

        // 장애물에 StrategyPatternObstacle 스크립트 부착
        strategyPatternObstacle = obstacle.AddComponent<StrategyPatternObstacle>();

        // 장애물 기본 변수 설정
        strategyPatternObstacle.SetObstacleValues(speed, spawnPos, destPos);

        // MovementStrategy 설정
        strategyPatternObstacle.SetMovementStartegy(movementStrategy);

        // AttributeStrategy 설정, attributeStrategies 리스트에 중복이 있으면 제거
        // attributeStrategies = attributeStrategies.Distinct().ToList();
        strategyPatternObstacle.SetAttributeStartegies(attributeStrategies);

        strategyPatternObstacle.gameObject.SetActive(false);

        if (isPreCreatedObstacle)
        {
            ActivateObstacleInGame();
        }
    }

    public void ActivateObstacleInGame()
    {
        // 재생성 방지
        if (!isActivate)
        {
            isActivate = true;
            strategyPatternObstacle.gameObject.SetActive(true);
            strategyPatternObstacle.ActivateObstacle();
        }
    }
}
