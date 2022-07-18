using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text TotalScoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject[] levels;

    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, RESET, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;

    public GameObject currentBall;
    GameObject _currentLevel;
    GameObject _currentPlayer;

    bool _isSwitchingState;
 

    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value; 
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set { _balls = value; 
            ballsText.text = "BALLS: " + _balls;
        }
    }



    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }


    public void SwitchState(State newState, float delay = 2)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = true;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                //ball count changed for training
                Balls = 1;
                _currentPlayer = Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.RESET:
                Cursor.visible = true;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                //ball count changed for training
                Balls = 1;
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentBall);
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                    Debug.Log("LOADLEVEL");
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                //Destroy(_currentPlayer);
                Destroy(_currentLevel);
                panelGameOver.SetActive(true);
                SwitchState(State.RESET);
                //Debug.Log("test");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.RESET:
                break;
            case State.PLAY:
                if (currentBall == null)
                {
                    if (Balls > 0)
                    {
                        Debug.Log("Balls " + Balls);
                        currentBall = Instantiate(ballPrefab);
                    }
                    else if (!_isSwitchingState)
                    {
                        SwitchState(State.GAMEOVER);
                        //Debug.Log("PLAY");
                    }
                }
                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                {
                    currentBall.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                PlayerPrefs.SetInt("totalscore", Score);
                TotalScoreText.text = "TOTAL SCORE: " + PlayerPrefs.GetInt("totalscore");

                /*
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }*/
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.RESET:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }


}
