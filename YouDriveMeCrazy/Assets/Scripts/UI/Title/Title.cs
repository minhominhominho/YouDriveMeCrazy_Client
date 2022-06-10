using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionMenuHolder;
    public GameObject scoreMenuHolder;
    public GameObject TutorialHolder;
    public GameObject AchievementHolder;

    public string SceneToLoad;
    public string SceneToLoad2;
    public string SceneToLoad3;

    void Start()
    {
        // SceneManager.LoadScene("Test");
        mainMenuHolder.SetActive(true);
        optionMenuHolder.SetActive(false);
        scoreMenuHolder.SetActive(false);
        TutorialHolder.SetActive(false);
        AchievementHolder.SetActive(false);

        SoundManager.Instance.PlayBgm(BGM.titleBgm);
    }
    void Update()
    {

    }
    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionMenuHolder.SetActive(false);
        scoreMenuHolder.SetActive(false);
        TutorialHolder.SetActive(false);
        AchievementHolder.SetActive(false);
    }
    public void OptionMenu()
    {
        mainMenuHolder.SetActive(false);
        optionMenuHolder.SetActive(true);
        scoreMenuHolder.SetActive(false);
        TutorialHolder.SetActive(false);
        AchievementHolder.SetActive(false);
    }
    public void ScoreMenu()
    {
        mainMenuHolder.SetActive(false);
        optionMenuHolder.SetActive(false);
        scoreMenuHolder.SetActive(true);
        TutorialHolder.SetActive(false);
        AchievementHolder.SetActive(false);
    }
    public void TutorialMenu()
    {
        mainMenuHolder.SetActive(false);
        optionMenuHolder.SetActive(false);
        scoreMenuHolder.SetActive(false);
        TutorialHolder.SetActive(true);
        AchievementHolder.SetActive(false);
    }

    // tmp
    public void AchievementMenu()
    {
        mainMenuHolder.SetActive(false);
        optionMenuHolder.SetActive(false);
        scoreMenuHolder.SetActive(false);
        TutorialHolder.SetActive(false);
        AchievementHolder.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
    public void LoadOptions()
    {
        SceneManager.LoadScene(SceneToLoad2);
    }
    public void LoadOScoreBoard()
    {
        SceneManager.LoadScene(SceneToLoad3);
    }
    public void GameExit()
    {
        Application.Quit();
    }

    // public void QuitGame()
    //     {
    //         Application.quitting;
    //     }
}
