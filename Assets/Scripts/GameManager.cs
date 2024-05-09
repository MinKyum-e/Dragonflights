using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME_PLAY,
    GAME_PAUSE,
    GAME_OVER,
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }



    public GameState gameState = GameState.GAME_PLAY;

    public UIGroup ui_group;

    private int _coin;
    public int coin
    {
        get { return _coin; }
        set 
        { 
            _coin = value;
            ui_group.coinText.text = instance._coin.ToString();
        }
    }

    private int _score;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            ui_group.scoreText.text = instance._score.ToString();
        }
    }

    private int _max_score;
    public int max_score
    {
        get { return _max_score; }
        set 
        { 
            _max_score = value;
            ui_group.maxScoreText.text = instance._max_score.ToString();
            
         }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            max_score = PlayerPrefs.GetInt("Max_Score", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        coin = 0;
        score = 2000;

    }

    void Update()
    {
        switch(gameState)
        {
            case GameState.GAME_PLAY:
                break;
            case GameState.GAME_PAUSE:
                break;
            case GameState.GAME_OVER:
                GameOver();
                break;
        }
    }

    private void GameOver()
    {
        if (max_score < score)
            PlayerPrefs.SetInt("Max_Score", score);

        SceneManager.LoadScene("Result");
    }
}
