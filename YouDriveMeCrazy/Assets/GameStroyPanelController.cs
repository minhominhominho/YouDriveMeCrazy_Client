using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameStroyPanelController : MonoBehaviour
{

    [SerializeField] private GameObject Stage1Story;
    [SerializeField] private GameObject Stage2Story;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (SavingData.presentStageNum == 1)
        {
            if (Stage1Story != null) { Stage1Story.SetActive(true); }
            if (Stage2Story != null) { Stage2Story.SetActive(false); }
        }
        */
        // by 상민, SavingData 동기화 함수 만든 후 SceneManager.GetActiveScene().buildIndex == 2 지우고 SavingData.presentStageNum == 1로 변경하기
        if(SceneManager.GetActiveScene().buildIndex == 2){
            if (Stage1Story != null) { Stage1Story.SetActive(true); }
            if (Stage2Story != null) { Stage2Story.SetActive(false); }
        }
        else{
            if (Stage1Story != null) { Stage1Story.SetActive(false); }
            if (Stage2Story != null) { Stage2Story.SetActive(true); }
        }
    }

    public void TurnOffStory(){
        if(PhotonNetwork.IsMasterClient){
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SyncGameStart", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void SyncGameStart()
    {
        if (Stage1Story != null) { Stage1Story.SetActive(false); }
        if (Stage2Story != null) { Stage2Story.SetActive(false); }
        GameManager.Instance.GameStart();
    }
}
