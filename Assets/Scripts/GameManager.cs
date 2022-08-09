using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool EnableTraining;
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

    GameObject _player;

    public GameObject[] levels;

    // 2 player game objects
    public GameObject middleWall;
    public GameObject ballPrefab1;
    public GameObject playerPrefab1;
    public GameObject ballPrefab2;
    public GameObject playerPrefab2;
    public Text scoreText1;
    public Text ballsText1;
    public Text levelText1;
    public Text scoreText2;
    public Text ballsText2;
    public Text levelText2;
    public Text TotalScoreText2_1;
    public Text TotalScoreText2_2;
    public GameObject[] levels2_1;
    public GameObject[] levels2_2;

    public GameObject panelMenu2;
    public GameObject panelPlay2;
    public GameObject panelLevelCompleted2_1;
    public GameObject panelLevelCompleted2_2;
    public GameObject panelEliminated1;
    public GameObject panelEliminated2;
    public GameObject panelGameOver2_1;
    public GameObject panelGameOver2_2;

    GameObject _currentBall1;
    GameObject _currentLevel1;
    public GameObject currentBall2;
    GameObject _currentLevel2;
    GameObject _player1;
    GameObject _player2;

    public bool levelCompleted; //for agent script


    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER, MENU2, INIT2, PLAY2, RESET2, LEVELCOMPLETED2_1, LEVELCOMPLETED2_2, LOADLEVEL2, LOADLEVEL2_1, LOADLEVEL2_2, ELIMINATED1, ELIMINATED2, GAMEOVER2 }
    State _state;

    public GameObject currentBall;
    GameObject _currentLevel;
    bool _isSwitchingState;

    // for Enable Training Mode
    private float paddleX = 18f; //Xvalue of the ball for player 2
    private float startTime;

    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            Debug.Log(_score);
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set
        {
            _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }
    }


    private int _score1;

    public int Score1
    {
        get { return _score1; }
        set
        {
            _score1 = value;
            scoreText1.text = "SCORE: " + _score1;
        }
    }

    private int _level1;

    public int Level1
    {
        get { return _level1; }
        set
        {
            _level1 = value;
            levelText1.text = "LEVEL: " + _level1;
        }
    }

    private int _balls1;

    public int Balls1
    {
        get { return _balls1; }
        set
        {
            _balls1 = value;
            ballsText1.text = "BALLS: " + _balls1;
        }
    }

    private int _score2;

    public int Score2
    {
        get { return _score2; }
        set
        {
            _score2 = value;
            scoreText2.text = "SCORE: " + _score2;
        }
    }

    private int _level2;

    public int Level2
    {
        get { return _level2; }
        set
        {
            _level2 = value;
            levelText2.text = "LEVEL: " + _level2;
        }
    }

    private int _balls2;

    public int Balls2
    {
        get { return _balls2; }
        set
        {
            _balls2 = value;
            ballsText2.text = "BALLS: " + _balls2;
        }
    }


    public void Play1Clicked()
    {
        SwitchState(State.INIT);
    }

    public void Menu2Clicked()
    {
        Loader.Load(Loader.Scene.TwoPlayerScene);
    }

    public void Menu1Clicked()
    {
        Loader.Load(Loader.Scene.OnePlayerScene);
    }

    public void Play2Clicked()
    {
        SwitchState(State.INIT2);
    }


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State newState, float delay = 0)
    {
        if (!_isSwitchingState)
        {
            StartCoroutine(SwitchDelay(newState, delay));
        }
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        Debug.Log(newState);
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            // Single Player Logic
            case State.MENU:
                Cursor.visible = true;
                panelMenu.SetActive(true);
                panelMenu2.SetActive(false);
                break;
            case State.INIT:
                Cursor.visible = true;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                _player = Instantiate(playerPrefab);
                //Debug.Log("is_switchingState: " + _isSwitchingState);
                StartCoroutine(SwitchDelay(State.LOADLEVEL, 0.5f));
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentBall);
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                StartCoroutine(SwitchDelay(State.LOADLEVEL, 2f)); //fix level transition
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    StartCoroutine(SwitchDelay(State.GAMEOVER, 0.5f)); //fix level transition
                }
                else
                {
                    Debug.Log("Load Level");
                    _currentLevel = Instantiate(levels[Level]);
                    StartCoroutine(SwitchDelay(State.PLAY, 0.5f)); //fix level transition
                }
                break;
            case State.GAMEOVER:
                Destroy(_player);
                Destroy(_currentLevel);
                panelGameOver.SetActive(true);
                break;

            // Two Player Logic
            case State.MENU2:
                Cursor.visible = true;
                panelMenu.SetActive(false);
                panelMenu2.SetActive(true);
                break;
            case State.INIT2:
                Cursor.visible = true;
                panelPlay2.SetActive(true);
                if (!EnableTraining)
                {
                    Score1 = 0;
                    Level1 = 0;
                    Balls1 = 3;
                    Score2 = 0;
                    Level2 = 0;
                    Balls2 = 3;
                }
                else
                {
                    // add a timer
                    startTime = Time.time;
                    //Debug.Log("Start *time: " + startTime);
                    Score1 = 0;
                    Level1 = 0;
                    Balls1 = 1;  //ball count changed for training
                    Score2 = 0;
                    Level2 = 0;
                    Balls2 = 1;  //ball count changed for training
                }
                middleWall.SetActive(true);
                _player1 = Instantiate(playerPrefab1);
                _player2 = Instantiate(playerPrefab2);
                StartCoroutine(SwitchDelay(State.LOADLEVEL2, 0.5f)); //for level transition
                break;
            case State.RESET2:
                // add a timer
                startTime = Time.time;
                //Debug.Log("Start *time: " + startTime);
                Cursor.visible = true;
                panelPlay2.SetActive(true);
                Score1 = 0;
                Level1 = 0;
                Balls1 = 1; //ball count changed for training
                Score2 = 0;
                Level2 = 0;
                Balls2 = 1; //ball count changed for training
                middleWall.SetActive(true);
                _player2 = Instantiate(playerPrefab2);
                StartCoroutine(SwitchDelay(State.LOADLEVEL2, 0.5f));
                break;
            case State.PLAY2:
                break;
            // for human player (left side player)
            case State.LEVELCOMPLETED2_1:
                Destroy(_currentBall1);
                Destroy(_currentLevel1);
                Level1++;
                panelLevelCompleted2_1.SetActive(true);
                StartCoroutine(SwitchDelay(State.LOADLEVEL2_1, 2f)); //fix level transition 
                break;
            // for AI player (right side player)
            case State.LEVELCOMPLETED2_2:
                //Debug.Log("level completed");
                Destroy(currentBall2);
                Destroy(_currentLevel2);
                Level2++;
                levelCompleted = true; // for training reward
                panelLevelCompleted2_2.SetActive(true);
                //StartCoroutine(SwitchDelay(.....));
                StartCoroutine(SwitchDelay(State.LOADLEVEL2_2, 2f)); //for level transition
                break;
            case State.LOADLEVEL2:
                _currentLevel1 = Instantiate(levels2_1[Level1]);
                _currentLevel2 = Instantiate(levels2_2[Level2]);
                StartCoroutine(SwitchDelay(State.PLAY2, 0.5f));
                break;
            case State.LOADLEVEL2_1:
                if (Level1 >= levels2_1.Length)
                {
                    StartCoroutine(SwitchDelay(State.GAMEOVER2, 0.5f));
                }
                else
                {
                    _currentLevel1 = Instantiate(levels2_1[Level1]);
                    StartCoroutine(SwitchDelay(State.PLAY2, 0.5f));
                }
                break;
            case State.LOADLEVEL2_2:
                // add a timer
                startTime = Time.time;
                if (Level2 >= levels2_2.Length)
                {
                    StartCoroutine(SwitchDelay(State.GAMEOVER2, 0.5f));
                }
                else
                {
                    _currentLevel2 = Instantiate(levels2_2[Level2]);
                    StartCoroutine(SwitchDelay(State.PLAY2, 0.5f));
                }
                break;
            case State.ELIMINATED1:
                panelEliminated1.SetActive(true);
                break;
            case State.ELIMINATED2:
                panelEliminated2.SetActive(true);
                break;
            case State.GAMEOVER2:
                Destroy(_currentBall1);
                Destroy(_currentLevel1);
                Destroy(currentBall2);
                Destroy(_currentLevel2);
                Destroy(_player1);
                Destroy(_player2);
                middleWall.SetActive(false);
                if (_score1 > _score2)
                {
                    panelGameOver2_1.SetActive(true);
                }
                else if (_score1 < _score2)
                {
                    panelGameOver2_2.SetActive(true);
                }
                if (EnableTraining)
                {
                    StartCoroutine(SwitchDelay(State.RESET2, 0.5f));
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            // Single Player Logic
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (currentBall == null)
                {
                    if (Balls > 0)
                    {
                        currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if (_currentLevel != null && _currentLevel.transform.childCount == 0)
                {
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
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;

            // Two Player Logic
            case State.INIT2:
                break;
            case State.PLAY2:
                Debug.Log("child count: " + _currentLevel2.transform.childCount);
                if (EnableTraining)
                {
                    // check time difference
                    if (Time.time - startTime > 600)
                    {
                        Debug.Log("Out of time: " + Time.time);
                        SwitchState(State.GAMEOVER2);
                    }
                }
                if (_currentBall1 == null)
                {
                    if (Balls1 > 0)
                    {
                        _currentBall1 = Instantiate(ballPrefab1);
                    }
                    else
                    {
                        if (!EnableTraining)
                        {
                            SwitchState(State.ELIMINATED1);
                        }
                    }
                }
                if (currentBall2 == null)
                {
                    if (Balls2 > 0)
                    {
                        currentBall2 = Instantiate(ballPrefab2);
                    }
                    else
                    {
                        if (EnableTraining)
                        {
                            SwitchState(State.GAMEOVER2);
                        }
                        else
                        {
                            SwitchState(State.ELIMINATED2);
                        }
                    }
                }
                if (_currentLevel1 != null && _currentLevel1.transform.childCount == 0)
                {
                    SwitchState(State.LEVELCOMPLETED2_1);
                }
                if (_currentLevel2 != null && _currentLevel2.transform.childCount == 0)
                {
                    if (EnableTraining)
                    {
                        // reset current ball position
                        currentBall2.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    Debug.Log("is switching state:    " + _isSwitchingState);
                    SwitchState(State.LEVELCOMPLETED2_2);
                }
                break;
            case State.LEVELCOMPLETED2_1:
                break;
            case State.LEVELCOMPLETED2_2:
                break;
            case State.LOADLEVEL2:
                break;
            case State.LOADLEVEL2_1:
                break;
            case State.LOADLEVEL2_2:
                break;
            case State.ELIMINATED1:
                //SwitchState(State.PLAY2);
                /*
                if (_score2 > _score1)
                {
                    SwitchState(State.GAMEOVER2);
                }*/
                
                if (currentBall2 == null)
                {
                    if (Balls2 > 0)
                    {
                        currentBall2 = Instantiate(ballPrefab2);

                    }
                    else
                    {
                        SwitchState(State.GAMEOVER2);
                    }
                }
                if (_currentLevel1 != null && _currentLevel1.transform.childCount == 0)
                {
                    SwitchState(State.LEVELCOMPLETED2_1);
                }
                if (_currentLevel2 != null && _currentLevel2.transform.childCount == 0)
                {
                    if (EnableTraining)
                    {
                        // reset current ball position
                        currentBall2.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    Debug.Log("is switching state:    " + _isSwitchingState);
                    SwitchState(State.LEVELCOMPLETED2_2);
                }
                break;
            case State.ELIMINATED2:
                //SwitchState(State.PLAY2);
                /*
                if (_score1 > _score2)
                {
                    SwitchState(State.GAMEOVER2);
                }*/
                
                if (_currentBall1 == null)
                {
                    if (Balls1 > 0)
                    {
                        _currentBall1 = Instantiate(ballPrefab1);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER2);
                    }
                }
                if (_currentLevel1 != null && _currentLevel1.transform.childCount == 0)
                {
                    SwitchState(State.LEVELCOMPLETED2_1);
                }
                if (_currentLevel2 != null && _currentLevel2.transform.childCount == 0)
                {
                    if (EnableTraining)
                    {
                        // reset current ball position
                        currentBall2.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    Debug.Log("is switching state:    " + _isSwitchingState);
                    SwitchState(State.LEVELCOMPLETED2_2);
                }
                break;
            case State.GAMEOVER2:
                PlayerPrefs.SetInt("totalscore1", Score1);
                PlayerPrefs.SetInt("totalscore2", Score2);
                TotalScoreText2_1.text = PlayerPrefs.GetInt("totalscore1") + " TO " + PlayerPrefs.GetInt("totalscore2");
                TotalScoreText2_2.text = PlayerPrefs.GetInt("totalscore1") + " TO " + PlayerPrefs.GetInt("totalscore2");
                if (!EnableTraining)
                {
                    if (Input.anyKeyDown)
                    {
                        SwitchState(State.MENU);
                    }
                }
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            // Single Player Logic
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
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

            // Two Player Logic
            case State.INIT2:
                panelMenu.SetActive(false);
                break;
            case State.PLAY2:
                break;
            case State.LEVELCOMPLETED2_1:
                panelLevelCompleted2_1.SetActive(false);
                break;
            case State.LEVELCOMPLETED2_2:
                panelLevelCompleted2_2.SetActive(false);
                break;
            case State.LOADLEVEL2:
                break;
            case State.LOADLEVEL2_1:
                break;
            case State.LOADLEVEL2_2:
                break;
            case State.ELIMINATED1:
                panelEliminated1.SetActive(false);
                break;
            case State.ELIMINATED2:
                panelEliminated2.SetActive(false);
                break;
            case State.GAMEOVER2:
                panelPlay2.SetActive(false);
                panelGameOver2_1.SetActive(false);
                panelGameOver2_2.SetActive(false);
                break;
        }
    }


}