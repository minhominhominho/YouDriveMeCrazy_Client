using System;
using System.Collections;
using System.Collections.Generic;
using ScoreBoard;
using TMPro;
using UnityEngine;
using Api;
using UnityEngine.Networking;

public class ScoreBoardManager : MonoBehaviour
{
    #region private Fields

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject content;

    private Scores[] scoreList;

    #endregion
    
    void Start()
    {
        StartCoroutine(Api.Api.LoadScores((data) =>
        {
            print(data.Length);
            
            scoreList = data;

            PrintScore();
        }));
    }

    #region private methods

    private void PrintScore()
    {
        string txt = "";

        for(int i=0; i<scoreList.Length; i++)
        {
            Scores score = scoreList[i];

            txt = i + "\t" + score.ToString() + "\n";

            TMP_Text obj = (TMP_Text) Instantiate(scoreText, Vector3.zero, Quaternion.identity);

            obj.SetText(txt);
            
            obj.transform.SetParent(content.transform);
        }
    }

    #endregion

    #region Public Methods

    public void InsertScore()
    {
        StartCoroutine(Api.Api.InsertScore("Lee", "Choi", "150.5", scores =>
        {
            
            Debug.Log(scores.ToString());
        }));
    }

    #endregion
}