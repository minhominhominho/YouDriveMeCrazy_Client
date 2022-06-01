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

    [Header("Audio")]
    #region Audio
    [SerializeField] private AudioSource bgmSpeaker;
    [SerializeField] private AudioClip gamePlaySound;
    [SerializeField] private AudioClip gameClearSound;
    [SerializeField] private AudioClip gameOverSound;
    [Space]
    [SerializeField] private AudioSource sfxSpeaker;
    [SerializeField] private AudioClip startEngineSound;
    [SerializeField] private AudioClip clearZoneEnterSound;
    [SerializeField] private AudioClip hitCarSound;
    #endregion

    [Header("UI")]
    #region UI
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private GameObject gameStroyPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOptionPanel;
    [SerializeField] private GameObject stageClearPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;
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
    private bool isGamePause = false;
    private float currentStageClearTime = 0;
    private int isClickedBy = 0; // 0 = no one click button, 1 = master client click button, 2 = participant client click button
    #endregion

    [HideInInspector]public int WiperCount;
    [HideInInspector]public int KlaxonCount;




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

        // by 상민, @정민호 이건 왜 만든거?
        if (Input.GetKeyDown(pauseKey))
        {
            Pause();
        }
    }

    // by 상연,
    // 게임 클리어 or 게임오버 시 업적 관련 정보들을 서버로 전송
    private void SendRecord()
    {
        // by 상연,
        // recordDto 변수는 임시로 작성해놓은 것
        // /Api/RecordDto 클래스 참고해서 실제 객체를 만들어야 함 
        // 근데 게임 클리어 했을 때도 보내야 함
        RecordDto recordDto = null;
        StartCoroutine(Api.Api.Record(recordDto, result =>
        {
            // 여기에 어떤 업적이 완료되었는 지 검사 후 화면에 표시하는 로직이 들어가면 됨
            Debug.Log("안녕하세요. 가정에 평화가 가득하기를 바랍니다.");

            // 어떤 업적을 달성했는지가 result 변수에 담겨있음
            // RecordResultDto 타입의 변수니까 RecordResultDto 클래스 참고
            if (result.AnimalKill) Debug.Log("동물 100마리 킬 업적 달성");
            // 위와 같이 분기 다 처리하면 됨
        }));
    }

    public void GameStart()
    {
        isGameStart = true;
        Time.timeScale = 1;

        if (bgmSpeaker != null && gamePlaySound != null)
        {
            bgmSpeaker.loop = true;
            bgmSpeaker.PlayOneShot(gamePlaySound);
        }
        if (sfxSpeaker != null && startEngineSound != null)
        {
            sfxSpeaker.loop = false;
            sfxSpeaker.PlayOneShot(startEngineSound);
        }

        if (gameStroyPanel != null) { gameStroyPanel.SetActive(false); }
        if (gamePlayPanel != null) { gamePlayPanel.SetActive(true); }
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

            if (sfxSpeaker != null && clearZoneEnterSound != null)
            {
                sfxSpeaker.loop = false;
                sfxSpeaker.PlayOneShot(clearZoneEnterSound);
            }


            // game clear
            if (SavingData.presentStageNum == 2)
            {
                float Stage1ClearTime = float.Parse(SavingData.timeReocrd);
                SavingData.timeReocrd = (Stage1ClearTime + currentStageClearTime).ToString();
                StartCoroutine(CallGameClear());

                // by 상연,
                // 클리어타임 서버에 전송
                StartCoroutine(Api.Api.InsertScore(SavingData.player1Name, SavingData.player2Name, SavingData.timeReocrd, scores =>
                    {
                        Debug.Log(scores.ToString());
                    })
                );

                // by 상연,
                // 업적 관련 정보 전송
                SendRecord();
            }
            else
            {
                SavingData.timeReocrd = currentStageClearTime.ToString();
                StartCoroutine(CallStageClear());
            }
        }
    }

    // by 상민, 다른 클래스에서 GameManager.Instance.GameOver() 호출, 경고음 재생 3초 후에 GameOverPanel 활성화
    public void GameOver(GameState gameState)
    {
        // by 상연,
        // 클락션, 와이퍼 작동 횟수 등 실제 클리어 데이터 넣어야 함
        string playerName = PhotonNetwork.IsMasterClient ? SavingData.player1Name : SavingData.player2Name;
        StartCoroutine(Api.Api.Record(new RecordDto(playerName, (int)gameState, 100, 10, 10, float.Parse(SavingData.timeReocrd)), dto => { print(dto.ToString()); }));

        if (!isGameEnd)
        {
            print("GameOver");
            isGameEnd = true;

            if (sfxSpeaker != null && gameOverSound != null)
            {
                sfxSpeaker.loop = false;
                sfxSpeaker.PlayOneShot(gameOverSound);
            }


            currentStageClearTime = 0;
            StartCoroutine(CallGameOver(gameState));



            // by 상연,
            // 업적 관련 정보 전송
            // SendRecord();
        }

    }


    public IEnumerator CallStageClear()
    {
        yield return new WaitForSeconds(3f);
        if (stageClearPanel != null)
        { stageClearPanel.SetActive(true); }


        if (sfxSpeaker != null && gameClearSound != null)
        {
            sfxSpeaker.loop = false;
            sfxSpeaker.PlayOneShot(gameClearSound);
        }

        int minute = (int)currentStageClearTime / 60;
        int second = (int)currentStageClearTime % 60;
        clearTimeText.text = $"Clear Time :  {minute} : {second}";
    }

    public IEnumerator CallGameClear()
    {
        yield return new WaitForSeconds(3f);
        if (gameClearPanel != null)
        { gameClearPanel.SetActive(true); }

        int minute = (int)currentStageClearTime / 60;
        int second = (int)currentStageClearTime % 60;
        finalClearTimeText.text = $"Clear Time :  {minute} : {second}";
    }

    public IEnumerator CallGameOver(GameState gameState)
    {
        yield return new WaitForSeconds(3f);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        switch (gameState)
        {
            case GameState.KillAnimal:
                gameOverReasonText.text = "You kill animal";
                break;
            case GameState.KillPeople:
                gameOverReasonText.text = "You hit people";
                break;
            case GameState.HitCar:
                gameOverReasonText.text = "You crash into a car";
                if (sfxSpeaker != null && hitCarSound != null)
                {
                    sfxSpeaker.loop = false;
                    sfxSpeaker.PlayOneShot(hitCarSound);
                }
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
    }

    #region GameFlowControl
    //by 상민, photonView.RPC() 을 통해 게임에 참가중인 모든 플레이어 함수를 호출
    //client가 방장일때 && 현재 접속한 플레이어가 2명일 때만 버튼 클릭 가능 (수정 가능)

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
        //by 상민, Pause를 누른 사람과 Resume을 누른 사람이 다를 경우, return;
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
        SavingData.presentStageNum = 1;
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

    #endregion

    #region SyncGamePlay

    // by 상민, Master,Client 모두 같은 isClickedBy(누가 버튼을 눌렀는가) 값을 가지기 위해 PunRPC로 SyncPause에서 동기화
    [PunRPC]
    private void SyncPause(int isClickedBy)
    {
        this.isClickedBy = isClickedBy;
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
        isClickedBy = 0;
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncNextStage()
    {
        if (bgmSpeaker != null) { bgmSpeaker.Stop(); }
        if (sfxSpeaker != null) { sfxSpeaker.Stop(); }
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncRestartStage()
    {
        if (bgmSpeaker != null) { bgmSpeaker.Stop(); }
        if (sfxSpeaker != null) { sfxSpeaker.Stop(); }
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
    }

    // by 상민, 새로운 씬 로드하기 전 현재 오브젝트 제거
    [PunRPC]
    private void SyncLeaveStage()
    {
        if (bgmSpeaker != null) { bgmSpeaker.Stop(); }
        if (sfxSpeaker != null) { sfxSpeaker.Stop(); }
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(1);
    }

    private void MatchSavingData()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncSavingData", RpcTarget.All, SavingData.player1Name, SavingData.player2Name, SavingData.presentStageNum, SavingData.timeReocrd);
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


    // by 상민, 방 나가면 자동으로 호출
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    #endregion
}
