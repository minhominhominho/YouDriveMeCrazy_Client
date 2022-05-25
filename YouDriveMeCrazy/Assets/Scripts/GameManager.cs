using System.Diagnostics;
using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public static class SavingData
{
    public static string timeReocrd = "0";
    public static int presentStageNum;
    public static string player1Name;
    public static string player2Name;
    //로그인 성공시 저장되는 이름
    public static string myName;
}


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    [SerializeField] private GameObject inputManager;

    [Header("Audio")]
    #region Audio
    [SerializeField] private AudioSource speaker;
    [SerializeField] private AudioClip gameOverSound;
    #endregion

    [Header("UI")]
    #region UI
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOptionPanel;
    [SerializeField] private GameObject stageClearPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;
    #endregion


    #region GameState
    public bool isGameEnd { get; private set; }
    private float currentStageClearTime = 0;
    private bool isPause = false;
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

    //by 상민, stage1일때 timeRecord 기록 초기화
    void Setup()
    {
        print("Stage" + SavingData.presentStageNum + " Start!");
        isGameEnd = false;

        currentStageClearTime = 0;
        if(SavingData.presentStageNum == 1) { SavingData.timeReocrd = "0"; }

        if (stageClearPanel != null) { stageClearPanel.SetActive(false); }
        if (gameClearPanel != null) { gameClearPanel.SetActive(false); }
        if (gameOverPanel != null) { gameOverPanel.SetActive(false); }
    }

    void Update()
    {
        #region Check Cheat Mode
        Cheat.updateCheatState();
        #endregion

        if (!isGameEnd && !isPause)
        {
            int minute = (int)currentStageClearTime / 60;
            int second = (int)currentStageClearTime % 60;
            timerText.text = $"{minute} : {second}";
            currentStageClearTime += Time.deltaTime;
        }

        if(Input.GetKeyDown(pauseKey)){
            Pause();
        }
    }

    // by상민, clearNum==2시 서버에 클리어 시간 인서트하고, 클리어 시간 표시 구현
    // 스테이지 1 클리어 시 리브&고넥스트
    // 스테이지 2 클리어 시 개인 점수 표시하고 리브만
    // 스코어보드는 타이틀에서만 볼 수 있음
    public void StageClear()
    {
        if (!isGameEnd)
        {
            isGameEnd = true;
            print("stage" + SavingData.presentStageNum + " clear!!");
            print("You took" + currentStageClearTime + "seconds!");

            if (SavingData.presentStageNum == 2)
            {
                float Stage1ClearTime = float.Parse(SavingData.timeReocrd);
                SavingData.timeReocrd = (Stage1ClearTime + currentStageClearTime).ToString();
                StartCoroutine(CallGameClear());
            }
            else
            {
                SavingData.timeReocrd = currentStageClearTime.ToString();
                StartCoroutine(CallStageClear());
            }
        }
    }

    // by 상민, 다른 클래스에서 GameManager.Instance.GameOver() 호출, 경고음 재생 3초 후에 GameOverPanel 활성화
    public void GameOver()
    {
        if (!isGameEnd)
        {
            print("GameOver");
            isGameEnd = true;

            speaker.loop = false;
            speaker.PlayOneShot(gameOverSound);
        
            currentStageClearTime = 0;
            StartCoroutine(CallGameOver());
        }

    }


    public IEnumerator CallStageClear()
    {
        yield return new WaitForSeconds(3f);
        if (stageClearPanel != null)
        { stageClearPanel.SetActive(true); }
    }

    public IEnumerator CallGameClear()
    {
        yield return new WaitForSeconds(3f);
        if (gameClearPanel != null)
        { gameClearPanel.SetActive(true); }
    }

    public IEnumerator CallGameOver()
    {
        yield return new WaitForSeconds(3f);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    #region GameFlowControl
    //by 상민, photonView.RPC() 을 통해 게임에 참가중인 모든 플레이어 함수를 호출
    //client가 방장일때 && 현재 접속한 플레이어가 2명일 때만 버튼 클릭 가능 (수정 가능)

    public void Pause()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncPause", RpcTarget.All);

            }
        }
    }

    public void Resume()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncResume", RpcTarget.All);
            }
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
        }
    }

    public void Restart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                speaker.Stop();
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncRestartStage", RpcTarget.All);
            }
        }
    }

    public void Leave()
    {
        Time.timeScale = 1;
        SavingData.presentStageNum = 1;
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncLeaveStage", RpcTarget.All);
            }
        }
    }

    #endregion

    #region SyncGamePlay
    
    [PunRPC]
    private void SyncPause()
    {
        Time.timeScale = 0;
        gamePlayPanel.SetActive(false);
        gameOptionPanel.SetActive(true);
    }

    [PunRPC]
    private void SyncResume()
    {
        Time.timeScale = 1;
        gamePlayPanel.SetActive(true);
        gameOptionPanel.SetActive(false);
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncNextStage()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncRestartStage()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncLeaveStage()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(1);
    }

    // by 상민, 방 나가면 자동으로 호출
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    #endregion
}

