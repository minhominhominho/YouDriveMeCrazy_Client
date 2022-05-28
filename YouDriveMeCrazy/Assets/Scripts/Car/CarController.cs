using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class CarController : MonoBehaviourPunCallbacks//, IPunObservable
{
    public static CarController carController;

    private bool isEnded = false;
    [SerializeField] private int maxSpeed;

    #region Player Input
    // Player1
    [HideInInspector] public bool isBreakPressing, isLeftTurnPressing, isRightTurnSignalPressing, isKlaxonPressing;

    //Player2
    [HideInInspector] public bool isAccelPressing, isRightTurnPressing, isLeftTurnSignalPressing, isWiperPressing;

    // Calculated Input Values
    public float keyMomentum = 3f;  // Key Input goes from 0 to 1 in 1/3f seconds
    private float accelValue;
    private float turnValue;
    private bool wasBreakPressed;
    #endregion

    [Header("UI")]
    #region UI
    public GameObject speedIndicatorArrow;
    public TextMeshProUGUI speedInKM;

    public Image rTurnSignalUI, lTurnSignalUI;
    public Color turnSignalColor;

    public GameObject inkjet;
    private Vector3 inkjetExtendedScale;


    // These will be deleted in the last game version
    public TextMeshProUGUI breakUI, leftTurnUI, rightTurnSignalUI, klaxonUI, accelUI, rightTurnUI, leftTurnSignalUI, wiperUI;
    #endregion

    [Header("Wheel Control")]
    #region Wheel Control
    public float engineForce = 900f;
    public float steerAngle = 30f;
    public float breakForce = 30000f;
    Rigidbody rb;
    public WheelCollider frontRightCollider, frontLeftCollider, rearRightCollider, rearLeftCollider;
    public Transform frontRightTransform, frontLeftTransform, rearRightTransform, rearLeftTransform;

    // used for lowering the center of mass of the car
    public Transform centerOfMass;

    // expose wheel rotate for the generic tracker
    public Quaternion frontWheelRot;
    public Quaternion rearWheelRot;
    #endregion


    private void Start()
    {
        carController = this;

        inkjetExtendedScale = inkjet.transform.localScale;
        inkjet.transform.localScale = Vector3.zero;
        inkjet.SetActive(true);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void Update()
    {
        if (!GameManager.Instance.isGameEnd)
        {
            calculateInput();
            UpdateWheelPhysics();
            UpdateWheelTransforms();
        }
        else
        {
            if (!isEnded)
            {
                isEnded = true;
                print(maxSpeed);
            }
        }

        updateUI();
    }

    private void calculateInput()
    {
        // Map isPressing variables to -1 ~ +1 float values
        accelValue += isAccelPressing ? keyMomentum * Time.deltaTime : -keyMomentum * Time.deltaTime;
        accelValue = Mathf.Clamp(accelValue, 0, 1);

        // turnValue += isLeftTurnPressing ? -keyMomentum * Time.deltaTime : keyMomentum * Time.deltaTime;
        // turnValue += isRightTurnPressing ? keyMomentum * Time.deltaTime : -keyMomentum * Time.deltaTime;
        // turnValue = Mathf.Clamp(turnValue, -1, 1);
        turnValue += isLeftTurnPressing ? -keyMomentum * Time.deltaTime : 0;
        turnValue += isRightTurnPressing ? keyMomentum * Time.deltaTime : 0;
        turnValue = Mathf.Clamp(turnValue, -1, 1);
    }

    private void updateUI()
    {
        if (isBreakPressing) { breakUI.color = Color.red; } else { breakUI.color = Color.black; }
        if (isLeftTurnPressing) { leftTurnUI.color = Color.red; } else { leftTurnUI.color = Color.black; }
        if (isRightTurnSignalPressing) { rightTurnSignalUI.color = Color.red; } else { rightTurnSignalUI.color = Color.black; }
        if (isKlaxonPressing) { klaxonUI.color = Color.red; } else { klaxonUI.color = Color.black; }

        if (isAccelPressing) { accelUI.color = Color.red; } else { accelUI.color = Color.black; }
        if (isRightTurnPressing) { rightTurnUI.color = Color.red; } else { rightTurnUI.color = Color.black; }
        if (isLeftTurnSignalPressing) { leftTurnSignalUI.color = Color.red; } else { leftTurnSignalUI.color = Color.black; }
        if (isWiperPressing) { wiperUI.color = Color.red; } else { wiperUI.color = Color.black; }


        // Update speed indicator UI
        float tempSpeed = rb.velocity.magnitude * 10;
        speedIndicatorArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(tempSpeed / 240 * -180 + 90, -90, 90)));
        string temp = ((int)tempSpeed).ToString();
        speedInKM.text = new string('0', 6 - temp.Length) + temp;
        // Update max speed
        if (tempSpeed > maxSpeed) { maxSpeed = (int)tempSpeed; }


        // Update turn signal UI
        if (isRightTurnSignalPressing) { rTurnSignalUI.color = turnSignalColor; } else { rTurnSignalUI.color = Color.white; }
        if (isLeftTurnSignalPressing) { lTurnSignalUI.color = turnSignalColor; } else { lTurnSignalUI.color = Color.white; }


        if (inkjet.transform.localScale.x != 0)
        {

        }

        // Eliminate inkjet when wiper btn is pressed
        if (isWiperPressing && inkjet.transform.localScale != Vector3.zero) { inkjet.transform.localScale = Vector3.zero; }

        // For test
        if (Input.GetKeyDown(KeyCode.O)) { coverWindow(); }
    }

    public void coverWindow()
    {
        StartCoroutine(extendInkjetScale());
    }

    private IEnumerator extendInkjetScale()
    {
        // Inkjet effect
        while (!isWiperPressing && inkjet.transform.localScale.x < inkjetExtendedScale.x)
        {
            inkjet.transform.localScale += inkjet.transform.localScale * 0.1f + Vector3.one * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Updates the wheel transforms.
    private void UpdateWheelTransforms()
    {
        Quaternion rotation;
        Vector3 position;

        frontLeftCollider.GetWorldPose(out position, out rotation);
        frontLeftTransform.position = position;
        frontLeftTransform.rotation = rotation;

        frontRightCollider.GetWorldPose(out position, out rotation);
        frontRightTransform.position = position;
        frontRightTransform.rotation = rotation;

        // update the front wheel rotation for generic tracker
        frontWheelRot = rotation;

        rearLeftCollider.GetWorldPose(out position, out rotation);
        rearLeftTransform.position = position;
        rearLeftTransform.rotation = rotation;

        rearRightCollider.GetWorldPose(out position, out rotation);
        rearRightTransform.position = position;
        rearRightTransform.rotation = rotation;

        // update the rear wheel rotation for generic tracker
        rearWheelRot = rotation;

        //// apply the rotation updates for remote copies
        frontLeftTransform.rotation = frontWheelRot;
        frontRightTransform.rotation = frontWheelRot;
        rearLeftTransform.rotation = rearWheelRot;
        rearRightTransform.rotation = rearWheelRot;
    }

    // Updates the wheel physics.
    private void UpdateWheelPhysics()
    {
        // accelerate
        rearRightCollider.motorTorque = accelValue * engineForce;
        rearLeftCollider.motorTorque = accelValue * engineForce;
        frontRightCollider.motorTorque = accelValue * engineForce;
        frontLeftCollider.motorTorque = accelValue * engineForce;

        // steer
        frontRightCollider.steerAngle = turnValue * steerAngle;
        frontLeftCollider.steerAngle = turnValue * steerAngle;

        // apply brakeTorque
        if (isBreakPressing && !wasBreakPressed)
        {
            wasBreakPressed = true;
            Debug.Log("Break");
            rearRightCollider.brakeTorque = breakForce;
            rearLeftCollider.brakeTorque = breakForce;
            frontRightCollider.brakeTorque = breakForce;
            frontLeftCollider.brakeTorque = breakForce;
        }

        // reset brakeTorque
        if (!isBreakPressing && wasBreakPressed)
        {
            wasBreakPressed = false;
            Debug.Log("Break stop");
            rearRightCollider.brakeTorque = 0;
            rearLeftCollider.brakeTorque = 0;
            frontRightCollider.brakeTorque = 0;
            frontLeftCollider.brakeTorque = 0;
        }
    }
}