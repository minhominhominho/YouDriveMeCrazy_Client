using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameStoryPanelController : MonoBehaviour
{

    [SerializeField] private GameObject Stage1Story;
    [SerializeField] private GameObject Stage2Story;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Stage1"){
            if (Stage1Story != null) { Stage1Story.SetActive(true); }
            if (Stage2Story != null) { Stage2Story.SetActive(false); }
        }
        else if (SceneManager.GetActiveScene().name == "Stage2"){
            if (Stage1Story != null) { Stage1Story.SetActive(false); }
            if (Stage2Story != null) { Stage2Story.SetActive(true); }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || Cheat.cheatMode)
                {
                    TurnOffStory();
                }
            }
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
