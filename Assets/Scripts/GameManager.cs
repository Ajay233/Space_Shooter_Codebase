using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    private UIManager _uIManager;
    private int _waveNumber = 0;
    private int _initialWaveSize = 4;
    private int _waveSize;
    private int _waveMultiplyer = 2;
    private SpawnManager _spawnManager;
    private AudioSource _uiAudioSource;
    [SerializeField]
    private AudioClip _affirmative;
  

    // Start is called before the first frame update
    void Start()
    {
        _isGameOver = false;
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.LogError("Unable to find the UIManager");
        } else
        {
            _uiAudioSource = _uIManager.GetComponent<AudioSource>();
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Unable to find Spawn_Manager");
        }
        
        _waveSize = _initialWaveSize;

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        //{
        //    SceneManager.LoadScene(1); // Load current game scene
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Time.timeScale = 0;
        //    _uIManager.OpenPauseMenu();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    UnPauseGame();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    UnPauseGame();
        //}

        if (Keyboard.current.rKey.wasPressedThisFrame && _isGameOver)
        {
            SceneManager.LoadScene(1); // Load current game scene
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseGame();
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            UnPauseGame();
        }

        if (_waveSize < 1)
        {
            _spawnManager.stopEnemySpawn();
            SetNewWaveSize();
            _uIManager.ShowWaveComplete();
            StartWave();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _uIManager.OpenPauseMenu();
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PlayAffirmative();
        _uIManager.ClosePauseMenu();

    }

    public void QuitGame()
    {
        PlayAffirmative();
        Application.Quit();
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuRoutine());
    }

    IEnumerator MainMenuRoutine()
    {
        Time.timeScale = 1;
        PlayAffirmative();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Main_Menu");
    }

    public void DecreaseWaveSize()
    {
        _waveSize--;
    }

    void SetNewWaveSize()
    {
        _waveSize = _initialWaveSize + (_waveMultiplyer * _waveNumber);
    }

    void StartWave()
    {
        _waveNumber++;
        _uIManager.ShowNewWaveBanner(_waveNumber);
        StartCoroutine(StartNextWaveRoutine());
    }

    IEnumerator StartNextWaveRoutine()
    {
        yield return new WaitForSeconds(9);
        _spawnManager.StartSpawn();
        _spawnManager.StartSpawnRoutines(_waveNumber);
    }

    void PlayAffirmative()
    {
        StartCoroutine(PlayAffirmativeRoutine());
    }

    IEnumerator PlayAffirmativeRoutine()
    {
        _uiAudioSource.clip = _affirmative;
        _uiAudioSource.Play();
        yield return new WaitForSeconds(1);
    }
}
