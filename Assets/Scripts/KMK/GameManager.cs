
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
    public GameObject[] hearts;
    private int heart_idx = 1;

    private UIGroup ui_group = null;

    //private 변수참조할때 _ 빼고 접근하면 됨 ex) coin = 100;
    private int _coin;
    private int _score;
    private int _max_score;
    public float distance;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        ui_group = GameObject.Find("ScoreGroup").GetComponent<UIGroup>();
        max_score = PlayerPrefs.GetInt("Max_Score", 0);
        coin = 0;
        score = 0;
        distance = 0;
    }


    private void Start()
    {


    }

    void Update()
    {
        score = 100000;
        distance += Time.deltaTime*100;
        switch(gameState)
        {
            case GameState.GAME_PLAY:
                GamePlay();
                break;
            case GameState.GAME_PAUSE:
                GamePause();
                break;
            case GameState.GAME_OVER:
                GameOver();
                break;
        }
    }

    private void GamePlay()
    {
        Time.timeScale = 1;
    }

    private void GamePause()
    {
        Time.timeScale = 0;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        if (max_score < score)
        {
            PlayerPrefs.SetInt("Max_Score", score);
        }
        PlayerPrefs.SetInt("Coin", coin + PlayerPrefs.GetInt("Coin", 0));
        PlayerPrefs.SetInt("CurCoin", coin);
        PlayerPrefs.SetInt("Distance", (int)distance);
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene("Result");
    }

    public void HeartChange()
    {
        if(heart_idx >=0)
        {
            hearts[heart_idx].SetActive(false);
            heart_idx--;
        }

    }


    public int coin
    {
        get { return _coin; }
        set
        {
            _coin = value;
            if (ui_group != null)
                ui_group.coinText.text = instance._coin.ToString();
        }
    }

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            if(ui_group != null)
                ui_group.scoreText.text = instance._score.ToString();
        }
    }
    public int max_score
    {
        get { return _max_score; }
        set
        {
            _max_score = value;
            if (ui_group != null)
                ui_group.maxScoreText.text = instance._max_score.ToString();

        }
    }
}
