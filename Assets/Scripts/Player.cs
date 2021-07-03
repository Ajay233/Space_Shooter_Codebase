using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Underscorse are used in front of variables to denote private variables
    [SerializeField] // The SerializeField attribute allows us to still see this in the inspector
    private float _speed = 8f;
    [SerializeField]
    private Laser _laserPrefab;
    public bool buttonDown;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = -1f;
    [SerializeField]
    private int _playerLives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    //private bool _speedBoostActive = false;
    private float _speedMultiplyer = 2f;
    private bool _shieldActive = false;
    private Transform _shield;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    private GameManager _gameManager;

    private Transform _rightEngine;
    private Transform _leftEngine;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private AudioClip _laserAudio;

    private AudioSource _audioSource;
    private Animator _playerAnimator;

    private Vector2 _inputVal;
    private Vector3 _movement;
    private InputActionReference _inputActionReference;
    private PlayerInput _playerInput;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Unable to find Spawn_Manager");
        }

        if (_uiManager == null)
        {
            Debug.LogError("Unable to find UI_Manager");
        }

        if (_gameManager == null)
        {
            Debug.LogError("Unable to find Game_Manager");
        }

        _rightEngine = transform.GetChild(2);
        if (_rightEngine == null)
        {
            Debug.LogError("Unable to find right engine");
        }

        _leftEngine = transform.GetChild(3);
        if (_leftEngine == null)
        {
            Debug.LogError("Unable to find left engine");
        }

        _rightEngine.gameObject.SetActive(false);
        _leftEngine.gameObject.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Unable to find Player AudioSource");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }

        _playerAnimator = gameObject.GetComponent<Animator>();
        if (_playerAnimator == null)
        {
            Debug.LogError("Unable to find Player Animator");
        }

        _playerInput = gameObject.GetComponent<PlayerInput>();

    }

    void OnMove(InputValue value) {
        _inputVal = value.Get<Vector2>();
    }


    // Update is called once per frame
    void Update()
    {
        //CalculateMovement();
        // if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        // {
        //     UseLaser();
        // }
        //_inputVal = _playerInput.actions["Move"].ReadValue<Vector2>();
        CalcMoveFromInputSystem();
       // AnimatePlayerTurns();
    }

    void CalculateMovement()
    {
        float horizontalInput;
        float verticalInput;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // ButtonDown doesn't seem to refresh its value each frame??
        //buttonDown = Input.GetButtonDown("Horizontal");

        //if (transform.position.y >= 5.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, 5.8f, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{

        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        // A simpler implementation would make use of the Math.Clamp method
        // Math.Clamp takes a coordinate, a min and a max value
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5.8f), 0);

        // This does wrapping, so the player can exit one side and re-enter the screen on the other side
        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

        // Best version below:
        // transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //    transform.Translate(Vector3.right * _speed * Time.deltaTime);
        //}

        //if (Input.GetAxis("Horizontal") < 0)
        //{
        //    transform.Translate(Vector3.left * _speed * Time.deltaTime);
        //}
    }


    // This method is assigned to the 'Move action and is executed whenever that action is triggered'
    public void GetInputVal(InputAction.CallbackContext context)
    {
        // Janky as if there is a delay with the joystick.  
        // To use this, pass in the following to GetInputVal: InputAction.CallbackContext context 
        //_inputVal = context.ReadValue<Vector2>();

        // Smooth but out of control
        _inputVal = _playerInput.actions["Move"].ReadValue<Vector2>();
        //_movement = new Vector3(_inputVal.x, _inputVal.y, 0).normalized;
        Debug.Log("Executing the move finction");
        //Debug.Log("x = " + context.ReadValue<Vector2>().x);
        //Debug.Log("y = " + context.ReadValue<Vector2>().y);
    }


    public void CalcMoveFromInputSystem()
    {
        // Janky, as if there is a delay with the joystick 
        //_inputVal = _playerInput.actions["Move"].ReadValue<Vector2>();
        _movement = new Vector3(_inputVal.x, _inputVal.y, 0).normalized;
        //Debug.Log("x = " + _inputVal.x);
        //Debug.Log("y = " + _inputVal.y);
        transform.Translate(_movement * _speed * Time.deltaTime);

        // Prevents going off screen vertically
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5.8f), 0);

        // This does wrapping, so the player can exit one side and re-enter the screen on the other side
        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    public void UseLaser()
    {

        //if (Input.GetKeyDown(KeyCode.Space) &&  Time.time > _nextFire)
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            if (_tripleShotActive)
            {
                Instantiate(_tripleShotPrefab, new Vector3(transform.position.x -0.44f, transform.position.y -0.5f, 0), Quaternion.identity);
            } else
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }

            _audioSource.Play();
            
           // transform.position + new Vector3(0, transform.position.y + 1, 0) ?? is this better?? 

           // save the time now so we know when the laser was fired
        }

    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        //_speedBoostActive = true;
        _speed *= _speedMultiplyer; 
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5);
        _speed /= _speedMultiplyer;
        //_speedBoostActive = false;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shield = transform.GetChild(0);
        if (_shield != null)
        {
            _shield.transform.gameObject.SetActive(true);
        }
    }

    public void DecreaseLifeBar()
    {

        if (_shieldActive)
        {
            _shieldActive = false;
            _shield.transform.gameObject.SetActive(false);
            return;
        }

        _playerLives--;
        _uiManager.UpdatePlayerLivesDisplay(_playerLives);
        ShowPlayerDamage(_playerLives);
        if (_playerLives == 0)
        {
            if (_spawnManager != null)
            {
                _spawnManager.stopEnemySpawn();
            }
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _uiManager.DisplayGameOver();
            _gameManager.GameOver();
        }
    }

    public void IncreaseLifeBar(int health)
    {
        this._playerLives += health;
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        
        if (_uiManager != null)
        {
            _uiManager.UpdateScoreText(_score);
        }
    }

    void ShowPlayerDamage(int playerLives)
    {
        switch (playerLives)
        {
            case 2: _rightEngine.gameObject.SetActive(true); break;
            case 1: _leftEngine.gameObject.SetActive(true); break;
            default: return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
            DecreaseLifeBar();
        }
    }

    void AnimatePlayerTurns()
    {

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _playerAnimator.ResetTrigger("OnRightTurnTrigger");
            //_playerAnimator.SetTrigger("OnResetRightTurnTrigger");
            _playerAnimator.SetTrigger("OnResetTurnAnimTrigger");
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _playerAnimator.ResetTrigger("OnLeftTurnTrigger");
            //_playerAnimator.SetTrigger("OnResetLeftTurnTrigger");
            _playerAnimator.SetTrigger("OnResetTurnAnimTrigger");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _playerAnimator.SetTrigger("OnLeftTurnTrigger");

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _playerAnimator.SetTrigger("OnRightTurnTrigger");
        }
    }
}
