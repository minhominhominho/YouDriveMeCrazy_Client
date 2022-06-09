using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Type { all = 0, bgm, gameSfx, carSfx, obstacleSfx }
public enum BGM { titleBgm = 0, waitingRoomBgm, stageBgm };
public enum GameSfx { gameFail = 0, stageClear, enterClearZone, police };
public enum CarSfx { carKlaxon = 0, carBreak, carWiper, startEngine };
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
    #endregion
    [Space]
    [SerializeField] private List<AudioClip> bgm;
    [SerializeField] private List<AudioClip> gameSfx;
    [SerializeField] private List<AudioClip> carSfx;
    [SerializeField] private List<AudioClip> obstacleSfx;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this){
                Destroy(this.gameObject);
            }
        }
    }

    private void Start() {
        if(SceneManager.GetActiveScene().buildIndex == 0){
            print("hello");
        }
        else{
            print("hello2");
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

    public void Stop(Type type)
    {
        switch (type)
        {
            case Type.bgm:
                bgmSpeaker.Stop();
                break;
            case Type.gameSfx:
                gameSfxSpeaker.Stop();
                break;
            case Type.carSfx:
                carSfxSpeaker.Stop();
                break;
            case Type.obstacleSfx:
                obstacleSfxSpeaker.Stop();
                break;
            case Type.all:
                bgmSpeaker.Stop();
                gameSfxSpeaker.Stop();
                carSfxSpeaker.Stop();
                obstacleSfxSpeaker.Stop();
                break;
            default:
                break;
        }
    }


}
