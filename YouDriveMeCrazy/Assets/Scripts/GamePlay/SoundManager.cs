using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public enum Type { all = 0, bgm, gameSfx, carSfx, obstacleSfx }
public enum BGM { titleBgm = 0, waitingRoomBgm, stageBgm };
public enum GameSfx { gameFail = 0, stageClear, enterClearZone, police };
public enum CarSfx { startEngine };
public enum ObstacleSfx { warning = 0, animalRunaway, dirt, hitCar };
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Speaker")]
    #region Speaker
    [SerializeField] private AudioSource bgmSpeaker;
    [SerializeField] private AudioSource gameSfxSpeaker;
    [SerializeField] private AudioSource carSfxSpeaker;
    [SerializeField] private AudioSource obstacleSfxSpeaker;
    [SerializeField] private AudioSource breakSfxSpeaker;
    [SerializeField] private AudioSource turnSignalSfxSpeaker;
    [SerializeField] private AudioSource klaxonSfxSpeaker;
    [SerializeField] private AudioSource wiperSfxSpeaker;
    #endregion

    [Space]
    [Header("Sound Control")]
    #region 
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider bgmAudioSlider;
    [SerializeField] private Slider sfxAudioSlider;
    #endregion

    [Header("AudioClip")]
    #region SFX
    [Space]
    [SerializeField] private List<AudioClip> bgm;
    [SerializeField] private List<AudioClip> gameSfx;
    [SerializeField] private List<AudioClip> carSfx;
    [SerializeField] private List<AudioClip> obstacleSfx;
    [SerializeField] private AudioClip breakSfx;
    [SerializeField] private AudioClip turnSignalSfx;
    [SerializeField] private AudioClip klaxonSfx;
    [SerializeField] private AudioClip wiperSfx;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void PlayBgm(BGM bgmNum)
    {
        bgmSpeaker.loop = true;
        bgmSpeaker.clip = bgm[(int)bgmNum];
        bgmSpeaker.Play();
    }
    public void PlayGameSfx(GameSfx sfxNum)
    {
        gameSfxSpeaker.loop = false;
        gameSfxSpeaker.PlayOneShot(gameSfx[(int)sfxNum]);
    }

    public void PlayCarSfx(CarSfx sfxNum)
    {
        carSfxSpeaker.loop = false;
        carSfxSpeaker.PlayOneShot(carSfx[(int)sfxNum]);
    }
    public void PlayObstacleSfx(ObstacleSfx sfxNum)
    {
        obstacleSfxSpeaker.loop = false;
        obstacleSfxSpeaker.PlayOneShot(obstacleSfx[(int)sfxNum]);
    }


    public void PlayWiper()
    {
        breakSfxSpeaker.loop = false;
        breakSfxSpeaker.PlayOneShot(wiperSfx);
    }

    public void PlayBreak()
    {
        breakSfxSpeaker.loop = false;
        breakSfxSpeaker.PlayOneShot(breakSfx);
    }

    public void PlayKlaxon()
    {
        klaxonSfxSpeaker.loop = true;
        klaxonSfxSpeaker.clip = klaxonSfx;
        klaxonSfxSpeaker.Play();
    }

    public void StopKlaxon()
    {
        klaxonSfxSpeaker.loop = false;
        klaxonSfxSpeaker.Stop();
    }

    public void PlayTurnSignal()
    {
        turnSignalSfxSpeaker.loop = true;
        turnSignalSfxSpeaker.clip = turnSignalSfx;
        turnSignalSfxSpeaker.Play();
    }

    public void StopTurnSignal()
    {
        turnSignalSfxSpeaker.loop = false;
        turnSignalSfxSpeaker.Stop();
    }

    public void StopAll()
    {
        gameSfxSpeaker.Stop();
        bgmSpeaker.Stop();
        carSfxSpeaker.Stop();
        obstacleSfxSpeaker.Stop();
        klaxonSfxSpeaker.Stop();
        turnSignalSfxSpeaker.Stop();
        breakSfxSpeaker.Stop();
    }

    public void StopSfx()
    {
        gameSfxSpeaker.Stop();
        carSfxSpeaker.Stop();
        obstacleSfxSpeaker.Stop();
        klaxonSfxSpeaker.Stop();
        turnSignalSfxSpeaker.Stop();
        breakSfxSpeaker.Stop();
    }

    public void BGMAudioControl()
    {
        float sound = bgmAudioSlider.value;
        //슬라이더 미니멈 값을 -40으로 설정해놔서(그 이하로 내려가면 깨진대)슬라이더로 -40가면 완전 뮤트로 만들라고

        if (sound == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", sound * 30 / 40);
    }

    public void SFXAudioControl()
    {
        float sound = sfxAudioSlider.value;
        //슬라이더 미니멈 값을 -40으로 설정해놔서(그 이하로 내려가면 깨진대)슬라이더로 -40가면 완전 뮤트로 만들라고

        if (sound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound * 30 / 40);
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

}
