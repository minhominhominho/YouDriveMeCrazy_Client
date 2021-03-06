using System.Diagnostics;
using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using Api;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Debug = UnityEngine.Debug;
using TMPro;


public static class SavingData
{
    public static string timeReocrd = "0";
    public static int presentStageNum;
    public static string player1Name;
    public static string player2Name;
    public static string myName;
}


public class GameManager : MonoBehaviourPunCallbacks
{
    public enum GameState { GameClear = 0, KillAnimal = 1, KillPeople = 2, HitCar = 3, LaneCross = 4, TrafficLightViolation = 5, MidLaneCross = 6, OutOfTheWay = 7 };

    public static GameManager Instance;
    [SerializeField] private GameObject inputManager;

    [Header("UI")]
    #region UI
    private KeyCode pauseKey;
    [SerializeField] private GameObject gameStroyPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOptionPanel;
    [SerializeField] private GameObject stageClearPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text newAchievementText;
    #endregion

    [Header("Time")]
    #region Time
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private TextMeshProUGUI finalClearTimeText;
    [SerializeField] private TextMeshProUGUI gameOverReasonText;
    #endregion


    #region GameState

    public bool isGameStart { get; set; }
    public bool isGameEnd { get; private set; }
    public bool isGamePause = false;
    private float currentStageClearTime = 0;
    private int isClickedBy = 0; // 0 = no one click button, 1 = master client click button, 2 = participant client click button
    #endregion



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Setup();
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0;
        PhotonNetwork.Instantiate(inputManager.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    //by ??????, stage1?????? timeRecord ?????? ?????????
    void Setup()
    {
        print("Stage" + SavingData.presentStageNum + " Start!");
        isGameStart = false;
        isGamePause = false;
        isGameEnd = false;

        currentStageClearTime = 0;
        if (SavingData.presentStageNum == 1) { SavingData.timeReocrd = "0"; }
        Time.timeScale = 0;

        if (gameStroyPanel != null) { gameStroyPanel.SetActive(true); }
        if (gamePlayPanel != null) { gamePlayPanel.SetActive(false); }
        if (gameOptionPanel != null) { gameOptionPanel.SetActive(false); }
        if (stageClearPanel != null) { stageClearPanel.SetActive(false); }
        if (gameClearPanel != null) { gameClearPanel.SetActive(false); }
        if (gameOverPanel != null) { gameOverPanel.SetActive(false); }

        SoundManager.Instance.PlayBgm(BGM.stageBgm);

        MatchSavingData();
    }



    void Update()
    {
        #region Check Cheat Mode
        Cheat.updateCheatState();
        #endregion

        if (isGameStart && !isGameEnd && !isGamePause)
        {
            int minute = (int)currentStageClearTime / 60;
            int second = (int)currentStageClearTime % 60;
            timerText.text = $"{minute} : {second}";
            currentStageClearTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
                {
                    PhotonView photonView = PhotonView.Get(this);
                    photonView.RPC("SyncGameOver", RpcTarget.All);
                }
            }
        }
    }

    void initializeSavingData()
    {
        SavingData.player1Name = "";
        SavingData.player2Name = "";
        SavingData.presentStageNum = 0;
        SavingData.timeReocrd = "";
    }
    // by ??????,
    // ?????? ????????? or ???????????? ??? ?????? ?????? ???????????? ????????? ??????
    private void SendRecord(GameState gameState)
    {
        string playerName = PhotonNetwork.IsMasterClient ? SavingData.player1Name : SavingData.player2Name;
        StartCoroutine(Api.Api.Record(new RecordDto(playerName, (int)gameState, CarController.carController.MaxSpeed_Accievement,
            CarController.carController.WiperCount_Accievement, CarController.carController.KlaxonCount_Accievement,
             gameState == GameState.GameClear ? float.Parse(SavingData.timeReocrd) : 3600f), dto =>
        {
            // by ??????,
            // ?????? ????????? ??????????????? ??? ?????? ??? ????????? ??????
            int count = dto.GetCount();

            if (count > 0)
            {
                newAchievementText.enabled = true;
                newAchievementText.text = count + " New Achievements!";
            }
        }));
    }

    public void GameStart()
    {
        isGameStart = true;
        Time.timeScale = 1;

        SoundManager.Instance.PlayCarSfx(CarSfx.startEngine);

        if (gameStroyPanel != null) { gameStroyPanel.SetActive(false); }
        if (gamePlayPanel != null) { gamePlayPanel.SetActive(true); }
    }

    // by??????, clearNum==2??? ????????? ????????? ?????? ???????????????, ????????? ?????? ?????? ??????
    // ???????????? 1 ????????? ??? ??????&????????????
    // ???????????? 2 ????????? ??? ?????? ?????? ???????????? ?????????
    // ?????????????????? ?????????????????? ??? ??? ??????
    public void StageClear()
    {
        if (!isGameEnd)
        {
            isGameEnd = true;
            print("stage" + SavingData.presentStageNum + " clear!!");
            print("You took" + currentStageClearTime + "seconds!");

            SoundManager.Instance.PlayGameSfx(GameSfx.enterClearZone);

            // game clear
            if (SavingData.presentStageNum == 2)
            {
                float Stage1ClearTime = float.Parse(SavingData.timeReocrd);
                SavingData.timeReocrd = (Stage1ClearTime + currentStageClearTime).ToString();
                StartCoroutine(CallGameClear());

                // by ??????,
                // ??????????????? ????????? ??????
                StartCoroutine(Api.Api.InsertScore(SavingData.player1Name, SavingData.player2Name, SavingData.timeReocrd, scores =>
                    {
                        Debug.Log(scores.ToString());
                    })
                );

                // by ??????,
                // ?????? ?????? ?????? ??????
                SendRecord(GameState.GameClear);
            }
            else
            {
                SavingData.timeReocrd = currentStageClearTime.ToString();
                StartCoroutine(CallStageClear());
            }
        }
    }

    // by ??????, ?????? ??????????????? GameManager.Instance.GameOver() ??????, ????????? ?????? 3??? ?????? GameOverPanel ?????????
    public void GameOver(GameState gameState)
    {
        // by ??????,
        // ?????????, ????????? ?????? ?????? ??? ?????? ????????? ????????? ????????? ???
        // @????????? ?????? maxSpeed
        SendRecord(gameState);

        if (!isGameEnd)
        {
            print("GameOver");
            isGameEnd = true;

            SoundManager.Instance.PlayGameSfx(GameSfx.police);

            currentStageClearTime = 0;
            StartCoroutine(CallGameOver(gameState));
        }
    }


    public IEnumerator CallStageClear()
    {
        yield return new WaitForSeconds(3f);
        if (stageClearPanel != null)
        { stageClearPanel.SetActive(true); }

        SoundManager.Instance.PlayGameSfx(GameSfx.stageClear);

        int minute = (int)currentStageClearTime / 60;
        int second = (int)currentStageClearTime % 60;
        clearTimeText.text = $"Clear Time   {minute} : {second}";
    }

    public IEnumerator CallGameClear()
    {
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.PlayGameSfx(GameSfx.stageClear);
        if (gameClearPanel != null) { gameClearPanel.SetActive(true); }
        yield return new WaitForSeconds(3f);
        if (gameClearPanel != null) { gameClearPanel.transform.Find("GameClearStory").gameObject.SetActive(false); }
        int minute = (int)currentStageClearTime / 60;
        int second = (int)currentStageClearTime % 60;
        finalClearTimeText.text = $"Clear Time   {minute} : {second}";
    }


    public IEnumerator CallGameOver(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.KillAnimal:
                gameOverReasonText.text = "You kill animal";
                SoundManager.Instance.PlayObstacleSfx(ObstacleSfx.hitCar);
                break;
            case GameState.KillPeople:
                gameOverReasonText.text = "You hit people";
                SoundManager.Instance.PlayObstacleSfx(ObstacleSfx.hitCar);
                break;
            case GameState.HitCar:
                gameOverReasonText.text = "You crash into a car";
                SoundManager.Instance.PlayObstacleSfx(ObstacleSfx.hitCar);
                break;
            case GameState.LaneCross:
                gameOverReasonText.text = "You cross the white lane";
                break;
            case GameState.TrafficLightViolation:
                gameOverReasonText.text = "You violate the traffic light";
                break;
            case GameState.MidLaneCross:
                gameOverReasonText.text = "You cross the mid lane";
                break;
            case GameState.OutOfTheWay:
                gameOverReasonText.text = "You have entered the wrong path";
                break;
        }

        yield return new WaitForSeconds(4f);
        SoundManager.Instance.PlayGameSfx(GameSfx.gameFail);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void MatchSavingData()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncSavingData", RpcTarget.All, SavingData.player1Name, SavingData.player2Name, SavingData.presentStageNum, SavingData.timeReocrd);
    }

    #region GameFlowControl
    //by ??????, photonView.RPC() ??? ?????? ????????? ???????????? ?????? ???????????? ????????? ??????
    //client??? ???????????? && ?????? ????????? ??????????????? 2?????? ?????? ?????? ?????? ?????? (?????? ??????)

    public void Pause()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
        {

            PhotonView photonView = PhotonView.Get(this);
            if (PhotonNetwork.IsMasterClient) { photonView.RPC("SyncPause", RpcTarget.All, 1); }
            else { photonView.RPC("SyncPause", RpcTarget.All, 2); }

        }
    }

    public void Resume()
    {
        //by ??????, Pause??? ?????? ????????? Resume??? ?????? ????????? ?????? ??????, return;
        if ((PhotonNetwork.IsMasterClient && isClickedBy == 2) || (!PhotonNetwork.IsMasterClient && isClickedBy == 1))
        {
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncResume", RpcTarget.All);
        }
    }

    public void Next()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                SavingData.presentStageNum += 1;
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncNextStage", RpcTarget.All);
            }
            MatchSavingData();
        }
    }

    public void Restart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncRestartStage", RpcTarget.All);
            }
            MatchSavingData();
        }
    }

    public void Leave()
    {
        Time.timeScale = 1;
        // ??? ?????? ???????????? presentNum ???????????? ??????
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncLeaveStage", RpcTarget.All);
            }
        }
        MatchSavingData();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncMainMenu", RpcTarget.All);
            }
        }
        MatchSavingData();
    }

    #endregion

    #region SyncGamePlay

    // by ??????, Master,Client ?????? ?????? isClickedBy(?????? ????????? ????????????) ?????? ????????? ?????? PunRPC??? SyncPause?????? ?????????
    [PunRPC]
    private void SyncPause(int isClickedBy)
    {
        this.isClickedBy = isClickedBy;
        isGamePause = true;
        SoundManager.Instance.StopSfx();
        Time.timeScale = 0;
        gamePlayPanel.SetActive(false);
        gameOptionPanel.SetActive(true);
    }

    [PunRPC]
    private void SyncResume()
    {
        Time.timeScale = 1;
        isGamePause = false;
        gamePlayPanel.SetActive(true);
        gameOptionPanel.SetActive(false);
        isClickedBy = 0;
    }

    [PunRPC]
    private void SyncGameOver()
    {
        StageClear();
    }

    // by ??????, ????????? ??? ???????????? ??? ?????? ???????????? ??????
    [PunRPC]
    private void SyncNextStage()
    {
        SoundManager.Instance.StopAll();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // by ??????, ????????? ??? ???????????? ??? ?????? ???????????? ??????
    [PunRPC]
    private void SyncRestartStage()
    {
        SoundManager.Instance.StopAll();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
    }

    // by ??????, ????????? ??? ???????????? ??? ?????? ???????????? ??????
    [PunRPC]
    private void SyncLeaveStage()
    {
        initializeSavingData();
        SoundManager.Instance.StopAll();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    [PunRPC]
    private void SyncMainMenu()
    {
        initializeSavingData();
        SoundManager.Instance.StopAll();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Title");
    }


    [PunRPC]
    private void SyncSavingData(string player1Name, string player2Name, int presentStageNum, string timeRecord)
    {
        SavingData.player1Name = player1Name;
        SavingData.player2Name = player2Name;
        SavingData.presentStageNum = presentStageNum;
        SavingData.timeReocrd = timeRecord;
        print(SavingData.player1Name + SavingData.player2Name + SavingData.presentStageNum + SavingData.timeReocrd);
    }


    // by ??????, ??? ????????? ???????????? ??????
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    #endregion
}

