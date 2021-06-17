using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject backGroundImg;
    [SerializeField]
    private GameObject coinImg;
    [SerializeField]
    private GameObject tapImg;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private GameObject coinCountText;
    [SerializeField]
    private GameObject highScoreText;
    [SerializeField]
    private GameObject soundBtn;
    [SerializeField]
    private GameObject restartBtn;

    [SerializeField]
    private GameObject player;

    private int curScore;
    private const float LOAD_TIME = 3;
    // Start is called before the first frame update
    public enum GameStatus
    {
        START,
        MAIN,
        END,
        LOADING
    }

    public GameStatus gameStatus;

    public static GameController Instance;

    public void PlayerDeath()
    {
        gameStatus = GameStatus.END;
        if (Mathf.CeilToInt(player.transform.position.z) > curScore)
        {
            curScore = Mathf.CeilToInt(player.transform.position.z);
            scoreText.GetComponent<Text>().text = Convert.ToString(curScore);
        }
        int highScore = PlayerPrefs.GetInt("high score", 0);
        if (highScore < curScore)
        {
            highScore = curScore;
            PlayerPrefs.SetInt("high score", highScore);
        }
        soundBtn.SetActive(true);
        highScoreText.SetActive(true);
        highScoreText.GetComponent<Text>().text = Convert.ToString(highScore);
        restartBtn.SetActive(true);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("main");
    }

    public void SoundTurn()
    {
        if (AudioListener.volume == 1)
        {
            soundBtn.GetComponent<Image>().color = Color.gray;
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("sound", 0);
        }
        else
        {
            soundBtn.GetComponent<Image>().color = Color.white;
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    private void LoadGame()
    {
        if(PlayerPrefs.GetInt("sound", 0) == 0)
        {
            soundBtn.GetComponent<Image>().color = Color.gray;
            AudioListener.volume = 0;
        }
        else
        {
            soundBtn.GetComponent<Image>().color = Color.white;
            AudioListener.volume = 1;
        }
        gameStatus = GameStatus.LOADING;
        coinImg.SetActive(false);
        //backGroundImg.SetActive(false);
        tapImg.SetActive(false);
        scoreText.SetActive(false);
        coinCountText.SetActive(false);
        highScoreText.SetActive(false);
        soundBtn.SetActive(false);
        restartBtn.SetActive(false);
        StartCoroutine(ShowStartWdn());
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Start()
    {
        PlayerPrefs.SetInt("coin count", 0);
        PlayerPrefs.SetInt("high score", 0);
        LoadGame();
    }

    IEnumerator ShowStartWdn()
    {
        yield return new WaitForSeconds(LOAD_TIME);
        gameStatus = GameStatus.START;
        backGroundImg.SetActive(false);
        coinImg.SetActive(true);
        tapImg.SetActive(true);
        coinCountText.GetComponent<Text>().text = Convert.ToString(PlayerPrefs.GetInt("coin count", 0));
        coinCountText.SetActive(true);
    }

    public void CoinsInc()
    {
        int coinCount = PlayerPrefs.GetInt("coin count", 0);
        coinCount++;
        PlayerPrefs.SetInt("coin count", coinCount);
        coinCountText.GetComponent<Text>().text = Convert.ToString(coinCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if (gameStatus == GameStatus.START)
            {
                tapImg.SetActive(false);
                gameStatus = GameStatus.MAIN;
                scoreText.SetActive(true);
                curScore = 0;
                scoreText.GetComponent<Text>().text = Convert.ToString(curScore);
            }
        }
        if (gameStatus == GameStatus.MAIN)
        {
            if (Mathf.CeilToInt(player.transform.position.z) > curScore)
            {
                curScore = Mathf.CeilToInt(player.transform.position.z);
                scoreText.GetComponent<Text>().text = Convert.ToString(curScore);
            }
        }

    }
}
