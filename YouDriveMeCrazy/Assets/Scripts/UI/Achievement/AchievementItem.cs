using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private GameObject noAchieve;
    [SerializeField] private GameObject achieve;

    public void On()
    {
        noAchieve.SetActive(false);
        achieve.SetActive(true);
    }

    public void Off()
    {
        noAchieve.SetActive(true);
        achieve.SetActive(false);
    }
}
