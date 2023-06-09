using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtCoins, txtVictoryConditions;
    [SerializeField] GameObject victoryCondition;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;


    [SerializeField] private GameObject pauseScreen;
    
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();

        // ONLY WORKS IN DEV MODE NOT WHEN BUILT
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void Awake()
    {

        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();

            return instance;
        }
    }

    public void UpdateCoinUI(int _coins, int _victoryCondition)
    {
        txtCoins.text = "Coins: " + _coins + " / " + _victoryCondition;
    }

    public void ShowVictoryCondition(int _coins, int _victoryCondition)
    {
        victoryCondition.SetActive(true);
        txtVictoryConditions.text = "You need " + (_victoryCondition - _coins) + " more coins";
    }

    public void HideVictoryCondition()
    {
        victoryCondition.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }


    #region Pause 
    public void PauseGame(bool status)
    {
        // if true pause game 
        pauseScreen.SetActive(status);

        Time.timeScale = System.Convert.ToInt32(!status);

        /*if(status) 
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;*/


    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    #endregion
}
