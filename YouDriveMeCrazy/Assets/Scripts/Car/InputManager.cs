using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // Player1
    public KeyCode brakeBtn, leftTurnBtn, rightTurnSignalBtn, klaxonBtn;
    private bool isBreakPressing, isLeftTurnPressing, isRightTurnSignalPressing,isKlaxonPressing;

    //Player2
    public KeyCode accelBtn, rightTurnBtn, leftTurnSignalBtn, wiperBtn;
    private bool isAccelPressing,isRightTurnPressing,isLeftTurnSignalPressing,isWiperPressing;


    void Update()
    {
        if (PhotonNetwork.IsMasterClient) { getPlayer1Input(); }
        else { getPlayer2Input(); }
    }

    private void getPlayer1Input()
    {
        if (Input.GetKeyDown(brakeBtn)) { isBreakPressing = true; }
        else if (Input.GetKeyUp(brakeBtn)) { isBreakPressing = false; }

        if (Input.GetKeyDown(leftTurnBtn)) { isLeftTurnPressing = true; }
        else if (Input.GetKeyUp(leftTurnBtn)) { isLeftTurnPressing = false; }

        if (Input.GetKeyDown(rightTurnSignalBtn) && !isLeftTurnSignalPressing) { isRightTurnSignalPressing = true; }
        else if (Input.GetKeyUp(rightTurnSignalBtn)) { isRightTurnSignalPressing = false; if (Input.GetKey(leftTurnSignalBtn)) { isLeftTurnSignalPressing = true; } }

        if (Input.GetKeyDown(klaxonBtn)) { isKlaxonPressing = true; }
        else if (Input.GetKeyUp(klaxonBtn)) { isKlaxonPressing = false; }

        #region Cheat mode
        if (Cheat.cheatMode)
        {
            if (Input.GetKeyDown(accelBtn)) { isAccelPressing = true; }
            else if (Input.GetKeyUp(accelBtn)) { isAccelPressing = false; }

            if (Input.GetKeyDown(rightTurnBtn)) { isRightTurnPressing = true; }
            else if (Input.GetKeyUp(rightTurnBtn)) { isRightTurnPressing = false; }

            if (Input.GetKeyDown(leftTurnSignalBtn) && !isRightTurnSignalPressing) { isLeftTurnSignalPressing = true; }
            else if (Input.GetKeyUp(leftTurnSignalBtn)) { isLeftTurnSignalPressing = false; if (Input.GetKey(rightTurnSignalBtn)) { isRightTurnSignalPressing = true; } }

            if (Input.GetKeyDown(wiperBtn)) { isWiperPressing = true; }
            else if (Input.GetKeyUp(wiperBtn)) { isWiperPressing = false; }

            CarController.carController.isAccelPressing = this.isAccelPressing;
            CarController.carController.isRightTurnPressing = this.isRightTurnPressing;
            CarController.carController.isLeftTurnSignalPressing = this.isLeftTurnSignalPressing;
            CarController.carController.isWiperPressing = this.isWiperPressing;
            CarController.carController.isBreakPressing = this.isBreakPressing;
            CarController.carController.isLeftTurnPressing = this.isLeftTurnPressing;
            CarController.carController.isRightTurnSignalPressing = this.isRightTurnSignalPressing;
            CarController.carController.isKlaxonPressing = this.isKlaxonPressing;
        }
        #endregion
    }

    private void getPlayer2Input()
    {
        if (Input.GetKeyDown(accelBtn)) { isAccelPressing = true; }
        else if (Input.GetKeyUp(accelBtn)) { isAccelPressing = false; }

        if (Input.GetKeyDown(rightTurnBtn)) { isRightTurnPressing = true; }
        else if (Input.GetKeyUp(rightTurnBtn)) { isRightTurnPressing = false; }

        if (Input.GetKeyDown(leftTurnSignalBtn) && !isRightTurnSignalPressing) { isLeftTurnSignalPressing = true; }
        else if (Input.GetKeyUp(leftTurnSignalBtn)) { isLeftTurnSignalPressing = false; if (Input.GetKey(rightTurnSignalBtn)) { isRightTurnSignalPressing = true; } }

        if (Input.GetKeyDown(wiperBtn)) { isWiperPressing = true; }
        else if (Input.GetKeyUp(wiperBtn)) { isWiperPressing = false; }
    }

    // For syncronizing inputs of two players
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (stream.IsReading)
            {
                this.isAccelPressing = (bool)stream.ReceiveNext();
                this.isRightTurnPressing = (bool)stream.ReceiveNext();
                this.isLeftTurnSignalPressing = (bool)stream.ReceiveNext();
                this.isWiperPressing = (bool)stream.ReceiveNext();

                CarController.carController.isAccelPressing = this.isAccelPressing;
                CarController.carController.isRightTurnPressing = this.isRightTurnPressing;
                CarController.carController.isLeftTurnSignalPressing = this.isLeftTurnSignalPressing;
                CarController.carController.isWiperPressing = this.isWiperPressing;
            }

            if (stream.IsWriting)
            {
                stream.SendNext(this.isBreakPressing);
                stream.SendNext(this.isLeftTurnPressing);
                stream.SendNext(this.isRightTurnSignalPressing);
                stream.SendNext(this.isKlaxonPressing);

                CarController.carController.isBreakPressing = this.isBreakPressing;
                CarController.carController.isLeftTurnPressing = this.isLeftTurnPressing;
                CarController.carController.isRightTurnSignalPressing = this.isRightTurnSignalPressing;
                CarController.carController.isKlaxonPressing = this.isKlaxonPressing;
            }
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            if (stream.IsReading)
            {
                this.isBreakPressing = (bool)stream.ReceiveNext();
                this.isLeftTurnPressing = (bool)stream.ReceiveNext();
                this.isRightTurnSignalPressing = (bool)stream.ReceiveNext();
                this.isKlaxonPressing = (bool)stream.ReceiveNext();

                CarController.carController.isBreakPressing = this.isBreakPressing;
                CarController.carController.isLeftTurnPressing = this.isLeftTurnPressing;
                CarController.carController.isRightTurnSignalPressing = this.isRightTurnSignalPressing;
                CarController.carController.isKlaxonPressing = this.isKlaxonPressing;
            }

            if (stream.IsWriting)
            {
                stream.SendNext(this.isAccelPressing);
                stream.SendNext(this.isRightTurnPressing);
                stream.SendNext(this.isLeftTurnSignalPressing);
                stream.SendNext(this.isWiperPressing);

                CarController.carController.isAccelPressing = this.isAccelPressing;
                CarController.carController.isRightTurnPressing = this.isRightTurnPressing;
                CarController.carController.isLeftTurnSignalPressing = this.isLeftTurnSignalPressing;
                CarController.carController.isWiperPressing = this.isWiperPressing;
            }
        }
    }
}