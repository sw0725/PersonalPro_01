using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singltrun<GameManager>
{
    static public bool gameOver = true;
    public float timer = 10000;
    public Action GameStarting;
    public Action GameEnding;

    int score = 0;
    float CurrentTimer = 10000;

    GameObject canvas;
    GameObject StartScreen;
    GameObject EndScreen;
    TextMeshProUGUI timerMsg;
    TextMeshProUGUI scoreMsg;
    TextMeshProUGUI FinalScoreMsg;
    public int Score
    {
        get => score;
        set
        {
            if (score != value)
            {
                score = Math.Min(value, 999999);
                scoreMsg.text = $"Score : {Score}";
            }
        }
    }

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().gameObject;
        StartScreen = canvas.transform.GetChild(6).gameObject;
        StartScreen.SetActive(true);
        EndScreen = canvas.transform.GetChild(7).gameObject;
        EndScreen.SetActive(false);
        timerMsg = canvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        scoreMsg = canvas.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        timerMsg.gameObject.SetActive(false);
        scoreMsg.gameObject.SetActive(false);
        FinalScoreMsg = EndScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        player.OnDie += GameOver;
    }

    private void Update()
    {
        if (!gameOver) 
        {
            CurrentTimer -= Time.time/5;
            timerMsg.text = $"TimeLeft : {Mathf.FloorToInt(CurrentTimer)} Sec";
            if (CurrentTimer < 0) 
            {
                gameOver = true;
                GameOver();
            }
        }
    }

    public void StartButton()
    {
        gameOver = false;
        GameStarting?.Invoke();
        StartScreen.SetActive(false);
        timerMsg.gameObject.SetActive(true);
        CurrentTimer = timer;
        timerMsg.text = $"TimeLeft : {CurrentTimer} Sec";
        scoreMsg.gameObject.SetActive(true);
        scoreMsg.text = $"Score : {Score}";
    }

    public void ReStartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        gameOver = true;
        EndScreen.SetActive(true);
        timerMsg.gameObject.SetActive(false);
        scoreMsg.gameObject.SetActive(false);
        FinalScoreMsg.text = $"Score : {Score}";
        CurrentTimer = timer;
    }

    public void ScoreUp(int newScore)
    {
        Score += newScore;
    }

    Player player;

    public Player Player
    {
        get
        {
            if (player == null)
            {
                OnInitialize();
            }
            return player;
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();
    }

}
