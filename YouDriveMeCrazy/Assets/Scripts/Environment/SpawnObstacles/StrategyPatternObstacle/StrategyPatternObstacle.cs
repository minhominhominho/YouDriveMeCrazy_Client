using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyPatternObstacle : MonoBehaviour
{
    private MovementStartegy movementStartegy;
    private List<AttributeStartegy> attributeStartegies = new List<AttributeStartegy>();

    private bool isActivate;

    private float speed;
    private Transform spawnPos;
    private Transform destPos;


    public void SetObstacleValues(float speed, Transform spawnPos, Transform destPos)
    {
        this.speed = speed;
        this.spawnPos = spawnPos;
        this.destPos = destPos;

        transform.LookAt(destPos.position, Vector3.up);
    }

    public void SetMovementStartegy(MovementEnum mEnum)
    {
        switch (mEnum)
        {
            case MovementEnum.Static:
                movementStartegy = new StaticMovementStrategy();
                break;
            case MovementEnum.ForwardMoving:
                movementStartegy = new ForwardMovementStrategy();
                break;
            case MovementEnum.StaccatoMoving:
                movementStartegy = new StaccatoMovementStartegy();
                break;
        }
    }

    public void SetAttributeStartegies(List<AttributeEnum> aEnums)
    {
        foreach (AttributeEnum aEnum in aEnums)
        {
            switch (aEnum)
            {
                // KlaxonInteractable은 웬만하면 static이랑만 쓰고, 다른 Attribute 쓰지 말기
                case AttributeEnum.KlaxonInteractable:
                    attributeStartegies.Add((AttributeStartegy)new KlaxonAttributeStartegy());
                    break;
                case AttributeEnum.Jumping:
                    attributeStartegies.Add((AttributeStartegy)new JumpingAttributeStartegy());
                    break;
                case AttributeEnum.Rotating:
                    attributeStartegies.Add((AttributeStartegy)new RotatingAttributeStartegy());
                    break;
            }
        }
    }

    public void ActivateObstacle()
    {
        isActivate = true;
    }

    void Update()
    {
        if (isActivate)
        {
            Transform newTransform = movementStartegy.updateTransform(transform, speed);
            transform.position = newTransform.position;
            transform.rotation = newTransform.rotation;

            foreach (AttributeStartegy a in attributeStartegies)
            {
                newTransform = a.Excute(transform);
                transform.position = newTransform.position;
                transform.rotation = newTransform.rotation;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActivate)
        {
            if (collision.collider.CompareTag("Car"))
            {
                GameManager.Instance.GameOver(GameManager.GameState.KillAnimal);
            }
        }
    }
}
