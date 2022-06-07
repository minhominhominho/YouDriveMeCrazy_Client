using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private GameObject honourGameObject;
    [SerializeField] private GameObject serialKillerGameObject;
    [SerializeField] private GameObject roadkillMasterGameObject;
    [SerializeField] private GameObject troubleMakerGameObject;
    [SerializeField] private GameObject outlowGameObject;
    [SerializeField] private GameObject worstDriverGameObject;
    [SerializeField] private GameObject touchDownGameObject;
    [SerializeField] private GameObject lostSheepGameObject;
    [SerializeField] private GameObject racerGameObject;
    [SerializeField] private GameObject cleanAsAWhistleGameObject;
    [SerializeField] private GameObject lairyBrokeGameObject;
    [SerializeField] private GameObject conquerorGameObject;
    [SerializeField] private GameObject bestDriverGameObject;
    
    void Start()
    {
        AchievementItem honour = honourGameObject.GetComponent<AchievementItem>();
        AchievementItem serialKiller = serialKillerGameObject.GetComponent<AchievementItem>();
        AchievementItem roadkillMaster = roadkillMasterGameObject.GetComponent<AchievementItem>();
        AchievementItem troubleMaker = troubleMakerGameObject.GetComponent<AchievementItem>();
        AchievementItem outlow = outlowGameObject.GetComponent<AchievementItem>();
        AchievementItem worstDriver = worstDriverGameObject.GetComponent<AchievementItem>();
        AchievementItem touchDown = touchDownGameObject.GetComponent<AchievementItem>();
        AchievementItem lostSheep = lostSheepGameObject.GetComponent<AchievementItem>();
        AchievementItem racer = racerGameObject.GetComponent<AchievementItem>();
        AchievementItem cleanAsAWhistle = cleanAsAWhistleGameObject.GetComponent<AchievementItem>();
        AchievementItem lairyBroke = lairyBrokeGameObject.GetComponent<AchievementItem>();
        AchievementItem conqueror = conquerorGameObject.GetComponent<AchievementItem>();
        AchievementItem bestDriver = bestDriverGameObject.GetComponent<AchievementItem>();

        if (false)
        {
            honour.On();
            serialKiller.On();
            roadkillMaster.On();
            // troubleMaker.On();
            outlow.On();
            worstDriver.On();
            // touchDown.On();
            lostSheep.On();
            racer.On();
            // cleanAsAWhistle.On();
            lairyBroke.On();
            conqueror.On();
            bestDriver.On();
        }
        else
        {
            StartCoroutine(Api.Api.GetRecord(SavingData.myName, dto =>
            {
                if(dto.AchievementCount) honour.On();
                if(dto.AnimalKill) serialKiller.On();
                if(dto.PedestrianKill) roadkillMaster.On();
                if(dto.CarAccident) troubleMaker.On();
                if(dto.IllegalLaneChange) outlow.On();
                if(dto.SignalViolation) worstDriver.On();
                if(dto.CenterLineViolation) touchDown.On();
                if(dto.OffPath) lostSheep.On();
                if(dto.MaxSpeed) racer.On();
                if(dto.WiperCount) cleanAsAWhistle.On();
                if(dto.KlaxonCount) lairyBroke.On();
                if(dto.ClearCount) conqueror.On();
                if(dto.MinClearTime) bestDriver.On();
            }));
        }
    }
}
