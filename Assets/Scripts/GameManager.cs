using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int collectedCoins, victoryCondition = 7;
    [SerializeField] public TMP_Text WINTEXT;


    private void Awake ()
    {
        if (instance  == null)
        {
            instance = this; 
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
    }

    private void  Start()
    {
        UIManager.MyInstance.UpdateCoinUI(collectedCoins, victoryCondition);
    }

    public void AddCoins(int _coins)
    {
        collectedCoins += _coins; 
        //collectedCoins = collectedCoins + 1; 
        UIManager.MyInstance.UpdateCoinUI(collectedCoins, victoryCondition);
    }

    public void Finish()
    {
        if(collectedCoins >= victoryCondition)
        {
            WINTEXT.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            UIManager.MyInstance.ShowVictoryCondition(collectedCoins, victoryCondition);
        }
    }
}
