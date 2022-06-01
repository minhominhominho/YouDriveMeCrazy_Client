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
    private int maxSpeed;
    private int requiredWiperPressing;

    #region Player Input
    // Player1
    [HideInInspector] public bool isBreakPressing, isLeftTurnPressing, isRightTurnSignalPressing, isKlaxonPressing;

    //Player2
    [HideInInspector] public bool isAccelPressing, isRightTurnPressing, isLeftTurnSignalPressing, isWiperPressing;
    private bool preIsWiperPressing;
    private int wiperCount;

    // Calculated Input Values
    public float keyMomentum = 3f;  // Key Input goes from 0 to 1 in 1/3f seconds
    private float accelValue;
    private float turnValue;
    private bool wasBreakPressed;
    #endregion

    [Header("Audio")]
    #region Audio
    [SerializeField] private AudioSource sfxSpeaker;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip klaxonSound;
    [SerializeField] private AudioClip wiperSound;
    [SerializeField] private AudioClip throwDirtSound;
    // by 상민, @정민호 어디에 추가해야 할지 모르겠어요
    // sfxSpeaker.loop = false;
    // sfxSpeaker.PlayOneShot(gameClearSound);
    #endregion

    [Header("UI")]
    #region UI
    public GameObject speedIndicatorArrow;
    public TextMeshProUGUI speedInKM;

    public Image rTurnSignalUI, lTurnSignalUI;
    public Color turnSignalColor;

    public GameObject inkjet;
    private Image inkjetImg;
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

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        initializeWiper();
    }

    void Update()
    {
        if (!GameManager.Instance.isGameEnd)
        {
            calculateMovementInput();
            UpdateWheelPhysics();
            UpdateWheelTransforms();

            updateWiper();
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
        updateSpeedIndicator();
    }



    // Will be deleted later
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
    }

    private void updateSpeedIndicator()
    {
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
    }

    #region Wiper
    // Start에서만 호출되는 초기화 메소드
    private void initializeWiper()
    {
        inkjetImg = inkjet.GetComponent<Image>();
        inkjetExtendedScale = inkjet.transform.localScale;
        inkjet.transform.localScale = Vector3.zero;
        inkjet.SetActive(true);
    }

    // 플레이어의 wiper 버튼 누른 횟수 확인
    private void updateWiper()
    {
        // Eliminate inkjet when wiper btn is pressed over requiredWiperPressing times
        if (inkjet.transform.localScale != Vector3.zero)
        {
            // 잉크가 남아있고, 버튼이 눌렸다면 알베도 값을 조금 줄여줌
            if (!preIsWiperPressing && isWiperPressing)
            {
                wiperCount += 1;
                inkjetImg.color = new Vector4(inkjetImg.color.r, inkjetImg.color.g, inkjetImg.color.b, inkjetImg.color.a - (float)0.5 / requiredWiperPressing);
            }
            preIsWiperPressing = isWiperPressing;

            // 해당 와이퍼존의 횟수만큼 버튼을 누르면 바로 없애줌
            if (requiredWiperPressing <= wiperCount)
            {
                wiperCount = 0;
                inkjet.transform.localScale = Vector3.zero;
                inkjetImg.color = new Vector4(inkjetImg.color.r, inkjetImg.color.g, inkjetImg.color.b, 1);
                GameManager.Instance.WiperCount += 1;
            }
        }
    }

    public void coverWindow(int requiredWiperPressing)
    {
        // 이미 존재하면 초기화하고 새로 만듦
        if (inkjet.transform.localScale != Vector3.zero)
        {
            wiperCount = 0;
            inkjet.transform.localScale = Vector3.zero;
            inkjetImg.color = new Vector4(inkjetImg.color.r, inkjetImg.color.g, inkjetImg.color.b, 1);
        }

        if(sfxSpeaker != null && throwDirtSound != null){
            sfxSpeaker.loop = false;
            sfxSpeaker.PlayOneShot(throwDirtSound);
        }


        this.requiredWiperPressing = requiredWiperPressing;
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
    #endregion

    #region Car Movement
    private void calculateMovementInput()
    {
        // Mapping 'isPressing' variables
        accelValue += isAccelPressing ? keyMomentum * Time.deltaTime : -keyMomentum * Time.deltaTime;
        accelValue = Mathf.Clamp(accelValue, 0, 1);

        turnValue += isLeftTurnPressing ? -keyMomentum * Time.deltaTime : 0;
        turnValue += isRightTurnPressing ? keyMomentum * Time.deltaTime : 0;
        turnValue = Mathf.Clamp(turnValue, -1, 1);
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
    #endregion
}