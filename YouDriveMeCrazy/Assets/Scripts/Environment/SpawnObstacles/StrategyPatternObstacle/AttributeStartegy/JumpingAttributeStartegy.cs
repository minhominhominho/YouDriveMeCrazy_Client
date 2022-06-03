using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAttributeStartegy : AttributeStartegy
{
    private float timer;
    private float orgHeight = -100;
    private float jumpingHeight = 1f;
    private bool isReached;


    public Transform Excute(Transform obstacle)
    {
        // 한번만 실행됨 - 초기 상태 저장
        if (orgHeight == -100)
        {
            orgHeight = obstacle.position.y;
            jumpingHeight += obstacle.position.y;
        }


        Transform newTransform = obstacle;

        if (newTransform.position.y >= jumpingHeight)
        {
            isReached = true;
        }
        if (newTransform.position.y <= orgHeight)
        {
            isReached = false;
        }

        if (!isReached)
        {
            newTransform.position += Vector3.up * (0.05f + (jumpingHeight - obstacle.position.y) / (jumpingHeight - orgHeight)) * 8 * Time.deltaTime;
        }
        else
        {
            newTransform.position += Vector3.down * (0.05f + 1 - (jumpingHeight - obstacle.position.y) / (jumpingHeight - orgHeight)) * 8 * Time.deltaTime;
        }

        return newTransform;
    }
}
